using UnityEngine;


[DisallowMultipleComponent]
public class WeaponShootEffect : MonoBehaviour
{

    private ParticleSystem shootEffectParticleSystem;

    private void Awake()
    {

        //load components
        shootEffectParticleSystem = GetComponent<ParticleSystem>();

    }


    //set the shoot effect from the passed in weapon shoot effect so and aim angle
    public void SetShootEffect(WeaponShootEffectSO shootEffect, float aimAngle)
    {

        //set shoot effect color gradient
        SetShootEffectColorGradient(shootEffect.colorGradient);

        //set shoot effect particle system starting values
        SetShootEffectParticleStartingValues(shootEffect.duration, shootEffect.startParticleSize, shootEffect.startParticleSpeed, shootEffect.startLifetime, shootEffect.effectGravity, shootEffect.maxParticleNumber);

        //set shoot effect particle system particle burst particle number
        SetShootEffectParticleEmission(shootEffect.emissionRate, shootEffect.burstParticleNumber);

        //set emmitter rotation
        SetEmmitterRotation(aimAngle);

        //set the particle sprite
        SetShootEffectParticleSprite(shootEffect.sprite);

        //set shoot effect lifetime min and max velocities
        SetShootEffectVelocityOverLifeTime(shootEffect.velocityOverLifetimeMin, shootEffect.velocityOverLifetimeMax);

    }

    //set the shoot effect particle color gradient
    private void SetShootEffectColorGradient(Gradient gradient)
    {

        //set color gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = shootEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;

    }


    //set the effect starting values
    private void SetShootEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {

        ParticleSystem.MainModule mainModule = shootEffectParticleSystem.main;

        //set particle system duration
        mainModule.duration = duration;

        //set particle starting size
        mainModule.startSize = startParticleSize;

        //set paricle starting speed
        mainModule.startSpeed = startParticleSpeed;

        //set particle stat lifetime 
        mainModule.startLifetime = startLifetime;

        //set the gravity
        mainModule.gravityModifier = effectGravity;

        //set max number of particles
        mainModule.maxParticles = maxParticles;

    }


    //set shoot effect particle system particle burst number
    private void SetShootEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {

        ParticleSystem.EmissionModule emissionModule = shootEffectParticleSystem.emission;

        //set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);
        
        //set particle emission rate 
        emissionModule.rateOverTime = emissionRate;

    }


    //set shoot effect particle sprite
    private void SetShootEffectParticleSprite(Sprite sprite)
    {

        //set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = shootEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);

    }


    //set the rotation of the emmitter to match the aim angle
    private void SetEmmitterRotation(float aimAngle)
    {

        transform.eulerAngles = new Vector3(0f, 0f, aimAngle);

    }


    //set the shoot effect velocity over lifetime
    private void SetShootEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {

        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = shootEffectParticleSystem.velocityOverLifetime;

        //define the min and max velocity values of x
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants; //set the mode to two constants
        minMaxCurveX.constantMin = minVelocity.x; //set the min velocity for x
        minMaxCurveX.constantMax = maxVelocity.x; //set the max velocity for x
        velocityOverLifetimeModule.x = minMaxCurveX; //velocity over lifetime for x

        //define the min and max velocity values of y
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y; //set the min velocity for y
        minMaxCurveY.constantMax = maxVelocity.y; //set the max velocity for y
        velocityOverLifetimeModule.y = minMaxCurveY; //velocity over lifetime for y

        //define the min and max velocity values of y
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z; //set the min velocity for z
        minMaxCurveZ.constantMax = maxVelocity.z; //set the max velocity for z
        velocityOverLifetimeModule.z = minMaxCurveZ; //velocity over lifetime for z



    }


}
