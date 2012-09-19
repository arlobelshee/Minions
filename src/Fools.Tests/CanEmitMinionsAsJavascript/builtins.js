
builtins = function(){};

builtins.Rope = function() {
	this.self = this;
	var foo = []
	for(var v in arguments) {
		foo.push(arguments[v]);
	}
	this.value = foo.join();
};

builtins.Rope.prototype.toString = function() {
	return this.self.value;
};

builtins.print = function(message) {
	alert(message.toString());
}

builtins.for_programmers = function(something) {
	return new builtins.Rope("int(" + something + ")");
};
