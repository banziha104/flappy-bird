using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BirdScript : MonoBehaviour 
{
	public float upForce;			//새가 y축 양의 방향으로 가는힘
	public float forwardSpeed;		//새가 x축 양의 방향으로 가는힘
	public bool isDead = false;		//죽었는지?
	
	Animator anim;					//애니메이션 클래스
	bool flap = false;				//화면이 클릭됬는지


	void Start()
	{
		anim = GetComponent<Animator> (); //시작시 애니메이션을 가져옮
		GetComponent<Rigidbody2D>().velocity = new Vector2 (forwardSpeed, 0); //Rigidbody 적용
	}

	void Update()
	{
		/*프레임 단위로 죽었는지 체크*/
		if (isDead)
			return;
		/*입력 감지*/
		if (Input.anyKeyDown)
			flap = true;
	}

	void FixedUpdate()
	{
		/*입력이 잇다면*/
		if (flap) 
		{
			flap = false; // 원상태로 돌림

			AudioClip clip = Resources.Load ("Sounds/Flap") as AudioClip; //음악 장입

			GameObject.Find ("Main Camera").GetComponent<AudioSource> ().PlayOneShot (clip); //음악 재생

			/*애니메이션 실행*/
			anim.SetTrigger("Flap");
			/*위로 튀어오름*/
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, upForce));
		}
	}

	/*충돌체*/
	void OnCollisionEnter2D(Collision2D other)
	{
		/*충돌했을 경우*/
		AudioClip clip = Resources.Load ("Sounds/Hit") as AudioClip;
 
		/*음악재생*/
		GameObject.Find ("Main Camera").GetComponent<AudioSource> ().PlayOneShot (clip);
		/*아직 살았을 경우*/
		if (isDead == false) {
			/*배경 재생*/
			AudioClip clip2 = Resources.Load ("Sounds/Fall") as AudioClip;
			GameObject.Find ("Main Camera").GetComponent<AudioSource> ().PlayOneShot (clip2);

		}

		/*죽었을때*/
		isDead = true;
		/*죽음 애니메이션 재생*/
		anim.SetTrigger ("Die");
		GameControlScript.current.BirdDied ();

	}
}
