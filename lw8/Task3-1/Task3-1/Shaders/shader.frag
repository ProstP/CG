#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 viewPos;

uniform int paraboloidCount;
uniform vec3 paraboloidPositions[10];
uniform float paraboloidSizes[10];
uniform float paraboloidHeights[10];

uniform float ambientStrength;
uniform float specularStrength;
uniform float shininess;

bool IsPointInParaboloid(vec3 point, vec3 position, float size, float height)
{
    if (point.y < position.y || point.y > position.y + height)
    {
        return false;
    }

    float dx = point.x - position.x;
    float dz = point.z - position.z;

    float paraboloidY = position.y + (dx * dx) / (size * size) + (dz * dz) / (size * size);

    return point.y >= paraboloidY;
}

bool IsPointInAnotherObject(vec3 point)
{
    for (int i = 0; i < paraboloidCount; i++)
    {
        if (IsPointInParaboloid(point, paraboloidPositions[i], paraboloidSizes[i], paraboloidHeights[i]))
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

    vec3 offsetByNormal = norm * 0.002;
    if(!IsInShadow(FragPos + offsetByNormal, lightDir))
    {
        result += CalculateDiffuse(norm, lightDir);
        result += CalculateShininess(norm, lightDir);
    }
    
    result *= Color;
    FragColor = vec4(result, 1.0);
}