var app = angular.module('myApp', ['ui.router', 'ngMaterial', 'ngAria', 'angular-loading-bar', 'ngAnimate', 'ngMdIcons']);


app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$mdIconProvider', '$mdThemingProvider',
  function ($stateProvider, $urlRouterProvider, $locationProvider, $mdIconProvider, $mdThemingProvider) {
  	$urlRouterProvider.otherwise("/");
  	$stateProvider
		.state('weather', {
			url: "/",
			templateUrl: 'app/views/weather.html',
			controller: 'weatherController',
			controllerAs: 'vm'
		});

  	$locationProvider.html5Mode(true);

  	$mdIconProvider.icon("search", "/Content/icons/search.svg", 24);
  	$mdIconProvider.icon("close", "/Content/icons/close.svg", 24);

  	$mdThemingProvider.theme('default')
			 .primaryPalette('teal')
			 .accentPalette('green');


  }]);