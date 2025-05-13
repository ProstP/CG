#version 330 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D textureFrom;
uniform sampler2D textureTo;
uniform vec2 Center;
uniform float Time;
uniform vec2 ClickPos;

void main()
{
    vec2 uv = texCoord;
    
    float dist = distance(uv, ClickPos);
    float wave = sin(dist * 20.0 - Time * 10.0) * exp(-dist * 5.0) * exp(-Time * 2.0);
    
    vec2 dir = normalize(uv - ClickPos);
    vec2 distortedUV = uv + dir * wave * 0.05;
    
    float progress = clamp(Time, 0.0, 1.0);
    vec4 colorFrom = texture(textureFrom, distortedUV);
    vec4 colorTo = texture(textureTo, distortedUV);
    
    float brightness = 1.0 - wave * 0.5;
    
    FragColor = mix(colorFrom, colorTo, progress) * brightness;
}
