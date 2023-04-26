using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ppsh : Weapon
{
    [SerializeField] private Player _player;

    private int _maxBullet = 71;
    private int _countBulletSpent;

    private void Awake()
    {
        _countBulletSpent = _maxBullet;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Game.IsMobile == false && Time.time > NextFire)
        {
            NextFire = Time.time + 0.04f / FireRate;
            Shoot();
        }

        if(ShootButton.IsHold && Game.IsMobile && Time.time > NextFire)
        {
            NextFire = Time.time + 0.04f / FireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Animator.SetTrigger("reload");
            _countBulletSpent = _maxBullet;
        }

        CurrentBullets.text = _countBulletSpent.ToString();
        MaxBullets.text = _maxBullet.ToString();
    }

    public override void Shoot()
    {
        _countBulletSpent--;

        if (_countBulletSpent >= 0)
        {

            RaycastHit hit;

            AudioSourse.PlayOneShot(ShootClip);

            Animator.SetTrigger("shoot");

            if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, Range))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                Explosion explosion = hit.transform.GetComponent<Explosion>();

                if (enemy != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * Force);
                    enemy.TakeDamage(Damage);
                }

                if (explosion != null)
                {
                    explosion.ExplodeDelay();
                }
            }
        }
        else if(_countBulletSpent < 0)
        {
            Animator.SetTrigger("reload");
            _countBulletSpent = _maxBullet;
        }
    }
}
