#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;

uniform int torusCount;
uniform vec3 torusPositions[10];
uniform float torusMajorRadii[10];
uniform float torusMinorRadii[10];

uniform float ambientStrength;
uniform float specularStrength;
uniform float shininess;

bool IsPointInTorus(vec3 point, vec3 position, float R, float r)
{
    vec3 p = point - position;

    float sumSquared = dot(p, p);
    float firstPart = sumSquared + R * R - r * r;
    float secondPart = 4.0 * R * R * (p.x * p.x + p.z * p.z);

    float val = firstPart * firstPart - secondPart;

    return val <= 0.001;
}

bool IsPointInAnotherObject(vec3 point)
{
    for (int i = 0; i < torusCount; i++)
    {
        if (IsPointInTorus(point, torusPositions[i], torusMajorRadii[i], torusMinorRadii[i]))
        {
            return true;
        }
    }

    return false;
}

bool IsInShadow(vec3 startPoint, vec3 direction)
{
    int numOfIterations = 100;

    for(int i = 0; i < numOfIterations; i++)
    {
        float progress = float(i) / float(numOfIterations);

        vec3 point = startPoint + direction * progress;
        
        if (IsPointInAnotherObject(point))
        {
            return true;
        }
    }

    return false;
}

vec3 CalculateAmbient()
{
    return ambientStrength * lightColor;
}

vec3 CalculateDiffuse(vec3 norm, vec3 lightDir)
{
    float diff = max(dot(norm, lightDir), 0.0);
    return diff * lightColor;
}

vec3 CalculateShininess(vec3 norm, vec3 lightDir)
{
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);

    float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
    return specularStrength * spec * lightColor;
}

void main()
{
    vec3 result = CalculateAmbient();    
    
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);

    vec3 offsetByNormal = norm * 0.05;
    if(!IsInShadow(FragPos + offsetByNormal, lightDir))
    {
        result += CalculateDiffuse(norm, lightDir);
        result += CalculateShininess(norm, lightDir);
    }
    
    result *= Color;
    FragColor = vec4(result, 1.0);
}