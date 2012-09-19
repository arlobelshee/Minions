
make_minion = function(name, action) {
	action.__name__ = name;
	execution_engine.add(action);
	return action;
};

Engine = function() {
	this.self = this;
	this.minions = [];
	return this;
};
Engine.prototype.begin = function() {
	for(var f in this.self.minions){
		this.self.minions[f]();
	}
};
Engine.prototype.add = function(minion) {
	this.self.minions.push(minion);
};

execution_engine = new Engine();
