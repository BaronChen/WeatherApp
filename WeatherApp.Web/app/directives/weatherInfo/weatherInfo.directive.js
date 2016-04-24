app.directive('weatherInfoDirective', [function () {

	return {
		restrict: 'E',
		scope: {
			currentWeather :  '='
		},
		controller: 'weatherInfoDirectiveController',
		controllerAs: 'ctrl',
		bindToController: true,
		templateUrl: 'app/directives/weatherInfo/weatherInfoDirective.html',
		link: function ($scope, element, attrs, ctrl) {

		}
	};
}
]);