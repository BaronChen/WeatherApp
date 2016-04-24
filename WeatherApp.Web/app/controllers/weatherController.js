app.controller('weatherController', ['$scope', 'weatherService', '$q', function ($scope, weatherService, $q) {

	var self = this;
	self.$scope = $scope;
	self.$scope.showError = false;
	self.$scope.disableCitySelect = false;
	self.weatherService = weatherService;
	self.$q = $q;

	$scope.cities = [];

	this.handleError = function (message) {

		$scope.errorMessage = message;

		$scope.showError = true;
	};

	this.hideErrorMessage = function () {
		self.$scope.showError = false;
	}

	this.queryCity = function (query) {
		var deferred = self.$q.defer();

		if (!query)
			return [];

		this.getCities(query).then(function (data) {
			deferred.resolve(data.Cities);
		}, function(message) {
			self.handleError(message);
			deferred.reject(message);
		});

		return deferred.promise;
	}


	this.selectedCityChange = function (item) {
		if (!item)
			return;
		self.$scope.selectedCity = item;
	}


	this.getCities = function (query) {

		return this.weatherService.getCitiesForQuery(query);
	}


	this.getWeather = function () {

		if (!self.$scope.selectedCity)
			self.handleError("A city need to be selected.");

		this.weatherService.getWeatherForCity(self.$scope.selectedCity.Id).then(function (data) {
			self.$scope.currentWeather = data;
		},
		this.handleError);
	}
}]);