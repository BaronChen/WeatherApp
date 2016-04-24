app.directive('headerDirective', ['$mdSidenav', function ($mdSidenav) {

		return {
			restrict: 'E',
			scope: {
			},
			templateUrl: 'app/directives/header/headerDirective.html',
			link: function($scope, element, attrs) {
				

				$scope.toggleMenu = function () {
					$mdSidenav('left').toggle();
				}

			}
		};
	}
]);