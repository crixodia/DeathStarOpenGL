#version 330 core
out vec4 FragColor;

uniform sampler2D texture_diffuse1;
//uniform sampler2D texture_diffuse2;

uniform sampler2D texture_specular1;    
uniform sampler2D texture_emission1;
uniform sampler2D texture_normals1;

struct Material {
    float shininess;
}; 

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

in vec3 FragPos;  
in vec3 Normal;  
in vec2 TexCoords;
  
uniform vec3 viewPos;
uniform Material material;
uniform Light light;

void main()
{
    // Normals
    vec3 normal = texture(texture_normals1, TexCoords).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    // Ambient
    vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
    //vec3 ambient2 = vec3(0.2, 0.3, 0.4) * texture(texture_diffuse2, TexCoords).rgb;
  	
    // Diffuse 
    //vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(texture_diffuse1, TexCoords).rgb;  
    
    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * texture(texture_specular1, TexCoords).rgb;  
	
	// Emission
	vec3 emission = texture(texture_emission1, TexCoords).rgb;
        
    vec3 result = /*ambient2 +*/ ambient + diffuse + specular + emission;
    FragColor = vec4(result, 1.0);
} 