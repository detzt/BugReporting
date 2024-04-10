using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTest : MonoBehaviour
{
    [SerializeField] public int numberOfParticles = 1000;

    private ParticleSystem stressParticle;

    private void Start()
    {
        stressParticle = this.gameObject.AddComponent<ParticleSystem>();
        InitializeStressParticles();
    }

    private void InitializeStressParticles()
    {
        ParticleSystem.MainModule mainModule = stressParticle.main;
        mainModule.maxParticles = numberOfParticles;
        mainModule.startSpeed = 10f;
        mainModule.startSize = 1f;
        
        // Color over lifetime
        ParticleSystem.ColorOverLifetimeModule colorLifetime = stressParticle.colorOverLifetime;
        
        colorLifetime.enabled = true;

        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0f), new GradientColorKey(Color.green, 0.5f), new GradientColorKey(Color.blue, 1f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
        );
        colorLifetime.color = grad;

        ParticleSystem.EmissionModule emissionModule = stressParticle.emission;
        emissionModule.rateOverTime = numberOfParticles;

        ParticleSystem.ShapeModule shapeModule = stressParticle.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Sphere;

        // Add force
        ParticleSystem.ForceOverLifetimeModule forceModule = stressParticle.forceOverLifetime;
        forceModule.enabled = true;
        forceModule.x = new ParticleSystem.MinMaxCurve(-10f, 10f);
        forceModule.y = new ParticleSystem.MinMaxCurve(-10f, 10f);
        forceModule.z = new ParticleSystem.MinMaxCurve(-10f, 10f);

        // Add collision
        ParticleSystem.CollisionModule collisionModule = stressParticle.collision;
        collisionModule.enabled = true;
        collisionModule.type = ParticleSystemCollisionType.World;
        collisionModule.mode = ParticleSystemCollisionMode.Collision3D;
        collisionModule.dampen = 0.5f;
        collisionModule.bounce = 0.5f;
        collisionModule.lifetimeLoss = 0f;
        collisionModule.minKillSpeed = 1f;

        // Setting particles as 3D circle objects (default Sprite-Packed-"Circle")
        ParticleSystemRenderer renderer = stressParticle.GetComponent<ParticleSystemRenderer>();
        renderer.material.shader = Shader.Find("Particles/Standard Unlit"); 
        renderer.renderMode = ParticleSystemRenderMode.Mesh;
        renderer.mesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
    }
}
