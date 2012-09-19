
my_function = function(input) {
	return new builtins.Rope("hello ", builtins.for_programmers(input))
}

hello_monster = make_minion("make_minion", function() {
	builtins.print(my_function(3));
});
