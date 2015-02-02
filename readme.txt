A take the McDonald's Observer Saga from Jimmy Bogard's NSB 2014 presentation "Scaling NServiceBus": http://fast.wistia.net/embed/iframe/y56svovwnk?popover=true

To run the solution:
- mark .ClassClient and .SagaEndpoint to startup
- hit F5
- press any key to create an order
- thread.sleeps used to make menu endpoints slower (but still faster than most McDonalds!)
- console shows menue item "stations" eventually bus.Replying back to Saga, and saga keeps track of what order items went to what menu stations and when all stations are complete, saga is complete

TODO:
=====
- add rest of menu items?
- break menu handlers out into their own endpoint
- add unit tests