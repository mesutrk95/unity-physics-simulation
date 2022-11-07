## **Unity Instant Physics Simulator**
### Overview
In this code sample, You see immediate physics calculation that can be used in linux server for online physics calculation.
As soon as an instance has been started, The instance makes an http server that other processes can have connection through the http protocol.

### Test
It listens to 5980 http port, And it starts the physics calculations by making an http response to :  

```
http://localhost:5980/Simulate
```
