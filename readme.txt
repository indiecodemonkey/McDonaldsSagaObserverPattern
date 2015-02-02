A take the McDonald's Observer Saga from Jimmy Bogard's NSB 2014 presentation "Scaling NServiceBus": http://fast.wistia.net/embed/iframe/y56svovwnk?popover=true

To run the solution:
- mark .ClassClient, .SagaEndpoint and .MenuStationEndpoint for startup
- hit F5
- press any key to create an order
- thread.sleeps used to make menu endpoints slower (but still faster than most McDonalds!)
- console shows menue item "stations" eventually bus.Replying back to Saga, and saga keeps track of what order items went to what menu stations and when all stations are complete, saga is complete

TODO:
=====
- add unit tests

DONE
====
- break menu handlers out into their own endpoint
- move to unobtrusive mode
- change Dictionary<Type, bool> to just a List<T>.
	- instead of storing all the items in a dictionary, and setting the value to false (false meaning, "has been made"), instead, just store a list of items.
	- add the list of items in the StartedBy handler.
	- as each menu station reports back, remove the item from the list
	- when teh list is empty, the order is ready