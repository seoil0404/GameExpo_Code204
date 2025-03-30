using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Attack Sounds By Character")]
    public AudioClip[] characterAttackSounds;

    [Header("SFX Clips")]
    public AudioClip spellCastSound;     
    public AudioClip bodyHitSound;         
    public AudioClip swordSwingSound;        
    public AudioClip dragonBreathSound;      
    public AudioClip fireArrowSound;

    [Header("Status Effect Sounds")]
    public AudioClip buffSound;
    public AudioClip debuffSound;     
    public AudioClip dodgeSound;    
    public AudioClip invincibleSound;   
    public AudioClip poisonSound;    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySpellCastSound()
    {
        PlaySound(spellCastSound, "Spell Cast");
    }

    public void PlayBodyHitSound()
    {
        PlaySound(bodyHitSound, "Body Hit");
    }

    public void PlaySwordSwingSound()
    {
        PlaySound(swordSwingSound, "Sword Swing");
    }

    public void PlayDragonBreathSound()
    {
        PlaySound(dragonBreathSound, "Dragon Breath");
    }

    public void PlayFireArrowSound()
    {
        PlaySound(fireArrowSound, "Arrow");
    }

    private void PlaySound(AudioClip clip, string name)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] {name} ���尡 �������� �ʾҽ��ϴ�.");
        }
    }

    public void PlayBuffSound()
    {
        PlaySound(buffSound, "���");
    }

    public void PlayDebuffSound()
    {
        PlaySound(debuffSound, "�϶�");
    }

    public void PlayDodgeSound()
    {
        PlaySound(dodgeSound, "ȸ��");
    }

    public void PlayInvincibleSound()
    {
        PlaySound(invincibleSound, "��ȿȭ");
    }

    public void PlayPoisonSound()
    {
        PlaySound(poisonSound, "��");
    }



    public void PlayAttackSound()
    {
        int index = GameData.SelectedCharacterIndex - 1;

        if (index < 0 || index >= characterAttackSounds.Length)
        {
            Debug.LogWarning($"[SoundManager] ��ȿ���� ���� ĳ���� �ε���: {index}");
            return;
        }

        AudioClip clip = characterAttackSounds[index];

        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] {index}�� ĳ������ ���� ���尡 �������� �ʾҽ��ϴ�.");
        }
    }
}
