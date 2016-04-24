'use strict';


app.service("weatherService", ['$http', '$q', function ($http, $q) {

	var self = this;

	this.getError = function(response) {
		if (response.data && response.data.Message) {
			return response.data.Message;
		} else {
			return "Sorry, there is an internal issue...";
		}
	}

	this.getDataWithUrl = function (url) {
		url = encodeURI(url);
		var deferred = $q.defer();

		$http({
			method: 'Get',
			url: url
		}).then(function (response) {
			deferred.resolve(response.data);

		}, function (response) {
			deferred.reject(self.getError(response));
		});

		return deferred.promise;
	}

	this.getCitiesForQuery = function (query) {

		var url = '/api/weather/cities?query='+query;

		return this.getDataWithUrl(url);
	}

	this.getWeatherForCity = function (cityId) {

		var url = '/api/weather/current?cityId=' + cityId;

		return this.getDataWithUrl(url);
	}



}]);