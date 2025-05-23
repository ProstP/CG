﻿#version 330 core
layout(location = 0) in vec3 aPosition;

out vec2 Position;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    Position = aPosition.xy;
}