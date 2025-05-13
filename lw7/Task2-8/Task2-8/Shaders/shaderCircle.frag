#version 330 core
out vec4 FragColor;

in vec2 Position;

uniform vec2 Center;
uniform float OuterRadius;
uniform float InternalRadius;
uniform vec3 BackroundColor;
uniform vec3 CircleColor;

bool IsPointPartOfCircle()
{
    float radiusSquared = dot(Position - Center, Position - Center);

    return (InternalRadius * InternalRadius <= radiusSquared && radiusSquared <= OuterRadius * OuterRadius);
}

vec3 GetColor()
{
    return IsPointPartOfCircle() ? CircleColor : BackroundColor;
}

void main()
{
    FragColor = vec4(GetColor(), 1.0);
}
