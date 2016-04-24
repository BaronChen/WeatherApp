
describe('weatherController', function () {
	beforeEach(module('myApp'));

	var $controller;
	var weatherService;
	var $timeout;
	var $q;

	beforeEach(inject(function (_$controller_, _$q_, _$timeout_) {
		$controller = _$controller_;
		$timeout = _$timeout_;
		$q = _$q_;
		weatherService = {};
	}));

	describe('#hanldeError()', function () {
		it('Show error message when there is an error', function () {
			var error = "Some errors";
			var $scope = {};
			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			controller.handleError(error);

			expect($scope.showError).toBe(true);
			expect($scope.errorMessage).toBe(error);

		});
	});

	describe('#hideError()', function () {
		it('Show hide error when get called', function () {
			var $scope = {};
			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			controller.hideErrorMessage();

			expect($scope.showError).toBe(false);
		});
	});

	describe('#queryCity()', function () {
		it('It should get cities base on query', function (done) {
			var $scope = {};
			weatherService.getCitiesForQuery = function (query) {
				var data = {
					"Cities": [
						{ "Country": "Australia", "City": "Archerfield Aerodrome" },
						{ "Country": "Australia", "City": "Amberley Aerodrome" },
						{ "Country": "Australia", "City": "Alice Springs Aerodrome" }
					]
				};

				var deferred = $q.defer();
				$timeout(function() {
					if (query)
						deferred.resolve(data);
					else {
						deferred.resolve([]);
					}
				}, 1000);
				
				return deferred.promise;
			};
			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			spyOn(weatherService, "getCitiesForQuery").and.callThrough();

			var result = controller.queryCity('Aerodrome');

			result.then(function(data) {
				expect(data.length).toBe(3);
				expect(data[0].City).toBe('Archerfield Aerodrome');
				done();
			}, function(message) {
				expect(message).toBe(null);
				done();
			});

			expect(weatherService.getCitiesForQuery).toHaveBeenCalled();
			$timeout.flush();

		});

		it('It should throw an error', function (done) {
			var $scope = {};
			var error = "some errors";

			weatherService.getCitiesForQuery = function () {
				var deferred = $q.defer();
				$timeout(function() {
					deferred.reject(error);
				}, 1000);
				return deferred.promise;
			};
			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			spyOn(weatherService, "getCitiesForQuery").and.callThrough();

			var result = controller.queryCity('Aerodrome');

			result.then(function (data) {
				expect(data).toBe(null);
				done();
			}, function (message) {
				expect(message).toBe(error);
				done();
			});

			expect(weatherService.getCitiesForQuery).toHaveBeenCalled();
			$timeout.flush();

		});

	});

	describe('#getWeather()', function () {
		it('It should get current weather.', function (done) {
			var $scope = {};
			weatherService.getWeatherForCity = function (cityId) {
				var data = {
					"Location": "Sydney, AU",
					"RelativeHumidity": "60%"
				}

				var deferred = $q.defer();
				deferred.resolve(data);
				return deferred.promise;
			};

			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			$scope.selectedCity= {Id : '12345'};

			spyOn(weatherService, "getWeatherForCity").and.callThrough();

			controller.getWeather();

			expect(weatherService.getWeatherForCity).toHaveBeenCalled();

			var deferred = $q.defer();

			$timeout(function () {
				deferred.resolve('Finish waiting');
			}, 1000);

			deferred.promise.then(function (value) {
				expect($scope.currentWeather).not.toBe(null);
				expect($scope.currentWeather.Location).toBe("Sydney, AU");
				expect($scope.currentWeather.RelativeHumidity).toBe("60%");
			}).finally(done);

			$timeout.flush();


		});

		it('It should get an error.', function (done) {
			var $scope = {};
			var messgae = "some errors";
			weatherService.getWeatherForCity = function (cityId) {
				var deferred = $q.defer();
				deferred.reject(messgae);
				return deferred.promise;
			};

			var controller = $controller('weatherController', { $scope: $scope, weatherService: weatherService });

			$scope.selectedCity = { Id: '12345' };

			spyOn(weatherService, "getWeatherForCity").and.callThrough();

			controller.getWeather();

			expect(weatherService.getWeatherForCity).toHaveBeenCalled();

			var deferred = $q.defer();

			$timeout(function () {
				deferred.resolve('Finish waiting');
			}, 1000);

			deferred.promise.then(function (value) {
				expect($scope.showError).toBe(true);
				expect($scope.errorMessage).toBe(messgae);
			}).finally(done);

			$timeout.flush();


		});
	});
});