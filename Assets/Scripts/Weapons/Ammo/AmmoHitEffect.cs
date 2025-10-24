using UnityEngine;

[DisallowMultipleComponent]
public class AmmoHitEffect : MonoBehaviour
{
    
    private ParticleSystem ammoHitEffectParticleSystem;

    private void Awake() 
    {

        ammoHitEffectParticleSystem = GetComponent<ParticleSystem>();

    }


    //set ammo hit effect from the passed in AmmoHitEffectSO details
    public void SetHitEffect(AmmoHitEffectSO ammoHitEffect)
    {

        //set the hit effect color gradient
        SetHitEffectColorGradient(ammoHitEffect.colorGradient);

        //set the hit effect particle system starting values
        SetHitEffectParticleStartingValues(ammoHitEffect.duration, ammoHitEffect.startParticleSize, ammoHitEffect.startParticleSpeed, ammoHitEffect.startLifetime, ammoHitEffect.effectGravity,
        ammoHitEffect.maxParticleNumber);

        //set hit effect particle system particle burst
        SetHitEffectParticleEmission(ammoHitEffect.emissionRate, ammoHitEffect.burstParticleNumber);

        //set the hit effect particle sprite
        SetHitEffectParticleSprite(ammoHitEffect.sprite);

        //set hit effect lifetime min and max velocities
        SetHitEffectVelocityOverLifeTime(ammoHitEffect.velocityOverLifetimeMin, ammoHitEffect.velocityOverLifetimeMax);

    }
    
    
    //set the hit effects particle system color gradient
    private void SetHitEffectColorGradient(Gradient gradient)
    {

        //set colour gradient
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = ammoHitEffectParticleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = gradient;

    }


    //set hit effect particle system starting values
    private void SetHitEffectParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
    {

        ParticleSystem.MainModule mainModule = ammoHitEffectParticleSystem.main;

        //set particle system duration
        mainModule.duration = duration;

        //set particle starting size
        mainModule.startSize = startParticleSize;

        //set the particle starting speed
        mainModule.startSpeed = startParticleSpeed;

        //set the starting particles lifetime
        mainModule.startLifetime = startLifetime;

        //set the particles starting gravity
        mainModule.gravityModifier = effectGravity;

        //set the max particles
        mainModule.maxParticles = maxParticles;
        
    }


    //set the hit effect particles system burst particle number
    private void SetHitEffectParticleEmission(int emissionRate, float burstParticleNumber)
    {

        ParticleSystem.EmissionModule emissionModule = ammoHitEffectParticleSystem.emission;

        //set particle burst number
        ParticleSystem.Burst burst = new ParticleSystem.Burst(0f, burstParticleNumber);
        emissionModule.SetBurst(0, burst);

        //set the particles emission rate
        emissionModule.rateOverTime = emissionRate;

    }


    //set the hit effect particle system sprite
    private void SetHitEffectParticleSprite(Sprite sprite)
    {

        //set particle burst number
        ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = ammoHitEffectParticleSystem.textureSheetAnimation;

        textureSheetAnimationModule.SetSprite(0, sprite);

    }

    
    //set the hit effect velocity over lifetime
    private void SetHitEffectVelocityOverLifeTime(Vector3 minVelocity, Vector3 maxVelocity)
    {

        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = ammoHitEffectParticleSystem.velocityOverLifetime;

        //define the min and max x velocity
        ParticleSystem.MinMaxCurve minMaxCurveX = new ParticleSystem.MinMaxCurve();
        minMaxCurveX.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveX.constantMin = minVelocity.x;
        minMaxCurveX.constantMax = maxVelocity.x;
        velocityOverLifetimeModule.x = minMaxCurveX;

        //define the min and max y velocity
        ParticleSystem.MinMaxCurve minMaxCurveY = new ParticleSystem.MinMaxCurve();
        minMaxCurveY.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveY.constantMin = minVelocity.y;
        minMaxCurveY.constantMax = maxVelocity.y;
        velocityOverLifetimeModule.y = minMaxCurveY;

        //define the min and max z velocity
        ParticleSystem.MinMaxCurve minMaxCurveZ = new ParticleSystem.MinMaxCurve();
        minMaxCurveZ.mode = ParticleSystemCurveMode.TwoConstants;
        minMaxCurveZ.constantMin = minVelocity.z;
        minMaxCurveZ.constantMax = maxVelocity.z;
        velocityOverLifetimeModule.z = minMaxCurveZ;

    }


}
