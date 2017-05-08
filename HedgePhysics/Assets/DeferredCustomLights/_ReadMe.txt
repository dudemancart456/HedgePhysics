This scene shows a basic implementation of "custom deferred lights" - i.e.
it shows how command buffers can be used to implement custom lights that
compute scene illumination in deferred shading.

The idea is: after regular deferred shading light pass is done,
draw a sphere for each custom light, with a shader that computes illumination
and adds it to the lighting buffer.

Note: this is small example code, and not a production-ready custom light system!

In this example, each custom light has a CustomLight.cs script attached; two kinds of
custom lights are implemented:

* Sphere: similar to a point light, but with an actual "area light size".
* Tube: a cylinder-shaped light, has a lengh along X axis and cylinder radius (size).

The shader to compute illumination is CustomLightShader.shader, and uses "closest point"
approximations Brian Karis' SIGGRAPH 2013 course
(http://blog.selfshadow.com/publications/s2013-shading-course/).

CustomLightRenderer.cs is a script that should be assigned to some "always visible"
object (e.g. ground). When that object becomes visible by any camera, it will add
command buffers with all the lights to that camera. This way this works for scene view
cameras too, but is not an ideal setup for a proper custom light system.

Caveats: for simplicity reasons, the example code does a not-too-efficient
rendering of light shapes. Would be more efficient to use stencil marking as in typical
deferred shading; here we just render the sphere backfaces with depth testing off.
