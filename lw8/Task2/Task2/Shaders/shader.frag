#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;

uniform int cubeCount;
uniform vec3 cubeCentres[10];
uniform float cubeSizes[10];

uniform float ambientStrength;
uniform float specularStrength;
uniform float shininess;

float distanceToCube(vec3 point, vec3 center, vec3 size)
{
    vec3 distance = abs(point - center) - size;
    return length(max(distance, 0.0)) + min(max(distance.x, max(distance.y, distance.z)), 0.0);
}

float minDistanceToAnotherObjects(vec3 p)
{
    float minDistance = 10000.0;
    for (int i = 0; i < cubeCount; i++)
    {
        float distance = distanceToCube(p, cubeCentres[i], vec3(cubeSizes[i] / 2));
        minDistance = min(minDistance, distance);
    }

    return minDistance;
}

bool isInShadow(vec3 startPoint, vec3 direction)
{
    float progress = 0.01;
    float maxDistance = length(lightPos - startPoint);

    float minDistance = 0.001;

    int numOfIterations = 100;

    for(int i = 0; i < numOfIterations; i++)
    {
        vec3 p = startPoint + direction * progress;
        float distance = minDistanceToAnotherObjects(p);
        
        if (distance < minDistance)
        {
            return true;
        }
        
        progress += distance;
        if (progress >= maxDistance) 
        {
            break;
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

    vec3 offsetByNormal = norm * 0.001;
    if(!isInShadow(FragPos + offsetByNormal, lightDir))
    {
        result += CalculateDiffuse(norm, lightDir);
        result += CalculateShininess(norm, lightDir);
    }
    
    result *= Color;
    FragColor = vec4(result, 1.0);
}