#pragma strict
var bulletprefab : Rigidbody; 
var shootforce : int = 20;
var shootclip : AudioClip;
var spawningpoint : GameObject;

	function Start () 
	{
	}
    
function Update () {

if (Input.GetButtonDown("Fire1"))
	{
	var Bullet : Rigidbody = Instantiate(bulletprefab, spawningpoint.transform.position , spawningpoint.transform.rotation);
	Bullet.velocity = spawningpoint.transform.TransformDirection(Vector3 (0,0,shootforce));
	GetComponent.<AudioSource>().PlayOneShot(shootclip); 
	} 
}


