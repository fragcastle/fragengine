# FragEngine & FragEd
FragCastle Games is porting [RockKickass](http://fragcastle.com/rock-kickass) to MonoGame, in doing so we're missing a lot of the niceities that we took for granted with ImpactJS. So, we're creating a game platform to make life a bit easier!

### FragEngine
FragEngine is the "guts" of our games. It handles collision detection, rendering, entitie classes, gravity, inertia, etc. This interfaces directly with MonoGame (and in some cases, even requires that we make changes to MonoGame)

### FragEd
FragEd is the editor that will ship with FragEngine. It allows you to create levels for your FragEngine games. It will create the necessary files that you can import into your game project and use at run-time.
