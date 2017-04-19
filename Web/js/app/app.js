var chatApp = angular.module('chatApp', ['ngRoute']);

chatApp.constant('signalr', {
	'connection': $.connection,
	'chat': $.connection.chatHub,
	'hub': $.connection.hub
});

chatApp.config(['$routeProvider', function ($routeProvider) {

	$routeProvider.
		when('/chat', {
			templateUrl: '../chat.html'
		}).
		otherwise('/chat');

	console.log('configured');

}]);

chatApp.controller('chatController', ['$scope', '$http', '$location', 'signalr', function ($scope, $http, $location, signalr) {

	var stateText = { 0: 'connecting', 1: 'connected', 2: 'reconnecting', 4: 'disconnected' };

	$scope.init = function() {
		console.log('initializing...');
		
		$scope.messages = [];
		$scope.state = stateText[4];
		$scope.url = 'http://localhost:8088/signalr';
		signalr.chat.client.receive = function (name, text) {
			console.log('received message ' + text + ' from ' + name);
			$scope.$apply(function () {
				$scope.messages.push({ name: name, text: text });
			});
		};
		signalr.hub.stateChanged(function(args) {
			console.log('connection state changed from ' + args.oldState + ' to ' + args.newState);
			var newState = stateText[args.newState];
			$scope.$applyAsync(function() {
				$scope.state = newState;
			});
		});
		console.log('initialized');
	}

	$scope.connect = function () {
		signalr.hub.stop();
		signalr.hub.url = $scope.url;
		signalr.hub.start($scope.name)
			.done(function () { console.log('Connected, connection ID=' + signalr.hub.id); })
			.fail(function () { console.log('Could not Connect!'); });
	}

	$scope.disconnect = function() {
		signalr.hub.stop();
	}

	$scope.send = function() {
		signalr.chat.server.send($scope.name, $scope.text);
	}

	$scope.clear = function() {
		$scope.messages.length = 0;
	}

	$scope.init();

}]);