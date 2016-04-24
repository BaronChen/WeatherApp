/**
 * Because of the time，I did not do a full set of unit test for the controller. 
 * The unit tests here is for demonstration purpose.  
 */

describe('weatherService', function () {
	beforeEach(module('myApp'));

	var $timeout;
	var $q;
	var weatherService;
	var getCitiesRequestHandler, getWeatherRequestHandler;
	var $httpBackend;

	beforeEach(inject(function (_$q_, _$timeout_, $injector,_weatherService_) {
		$timeout = _$timeout_;
		$q = _$q_;
		
		$httpBackend = $injector.get('$httpBackend');
		var cities = {
			"Cities": [{ "Country": "Australia", "City": "Archerfield Aerodrome", "Id":"12345" },
						{ "Country": "Australia", "City": "Amberley Aerodrome", "Id": "54321" },
						{ "Country": "Australia", "City": "Alice Springs Aerodrome", "Id": "90899" }]
		};
		getCitiesRequestHandler = $httpBackend.when('GET', '/api/weather/cities?query=Aerodrome')
							   .respond(cities);

		var currentWeather = {
			"Location": "Sydney, AU",		
			"RelativeHumidity": "60%"
		};
		getWeatherRequestHandler = $httpBackend.when('GET', '/api/weather/current?cityId=12345')
							   .respond(currentWeather);

		weatherService = _weatherService_;
	}));

	afterEach(function () {
		$httpBackend.verifyNoOutstandingExpectation();
		$httpBackend.verifyNoOutstandingRequest();
	});

	describe('#getCitiesForCountry()', function () {

		it('Show be able to get list of countries', function () {
		
			$httpBackend.expectGET('/api/weather/cities?query=Aerodrome');

			weatherService.getCitiesForQuery('Aerodrome').then(function (data) {
				expect(data.Cities.length).toBe(3);
			}, function(errorMsg) {
				expect(errorMsg).toBeNull();
			});

			$httpBackend.flush();


		});

		it('Show throw an error', function () {

			getCitiesRequestHandler.respond(400, {Message: 'Some error'});

			$httpBackend.expectGET('/api/weather/cities?query=Aerodrome');

			weatherService.getCitiesForQuery('Aerodrome').then(function (data) {
				expect(data).toBeNull();
			}, function (errorMsg) {
				expect(errorMsg).toBe('Some error');
			});

			$httpBackend.flush();


		});
	});


	describe('#getWeatherForCity()', function () {

		it('Show be able to get current weather', function () {

			$httpBackend.expectGET('/api/weather/current?cityId=12345');

			weatherService.getWeatherForCity('12345').then(function (data) {
				expect(data.Location).toBe("Sydney, AU");
				expect(data.RelativeHumidity).toBe("60%");
			}, function (errorMsg) {
				expect(errorMsg).toBeNull();
			});

			$httpBackend.flush();


		});

		it('Show throw an error', function () {

			getWeatherRequestHandler.respond(400, { Message: 'Some error' });

			$httpBackend.expectGET('/api/weather/current?cityId=12345');

			weatherService.getWeatherForCity('12345').then(function (data) {
				expect(data).toBeNull();
			}, function (errorMsg) {
				expect(errorMsg).toBe('Some error');
			});

			$httpBackend.flush();


		});
	});


});