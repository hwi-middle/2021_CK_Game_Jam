using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeSlidesUp : MonoBehaviour
{
    private Text text;
    private string str = "using System.Collections;\r\nusing System.Collections.Generic;\r\nusing UnityEngine;\r\n\r\npublic class EnemyController : MonoBehaviour\r\n{\r\n    public bool isNormal;\r\n\r\n    private GameObject target;\r\n    [HideInInspector] public bool isSlained;\r\n\r\n    void Start()\r\n    {\r\n\r\n        SetTarget();\r\n    }\r\n\r\n    void Update()\r\n    {\r\n        if (isSlained)\r\n        {\r\n            StartCoroutine(EnemyDie());\r\n        }\r\n    }\r\n\r\n    private void SetTarget()\r\n    {\r\n        if (isNormal)\r\n        {\r\n            float speed = Random.Range(0.8f, 1.2f);\r\n\r\n            target = GameObject.FindWithTag(\"Player\");\r\n\r\n            Vector3 targetVector = target.transform.position - transform.position;\r\n\r\n            Rigidbody2D rbd = GetComponent<Rigidbody2D>();\r\n            rbd.velocity = new Vector2(targetVector.normalized.x * speed, targetVector.normalized.y * speed);\r\n\r\n            Vector3 len = transform.position - target.transform.position;\r\n            float angle = Mathf.Atan2(len.x, len.y) * Mathf.Rad2Deg;\r\n            transform.eulerAngles = new Vector3(0, 0, -angle);\r\n        }\r\n\r\n        else\r\n        {\r\n            float speed = Random.Range(1f, 1.4f);\r\n\r\n            target = GameObject.FindWithTag(\"Player\");\r\n\r\n            Vector3 targetPosition = new Vector3(\r\n                Random.Range(-PlayerPrefs.GetFloat(\"x-Coordinate\"), PlayerPrefs.GetFloat(\"x-Coordinate\")),\r\n                Random.Range(-PlayerPrefs.GetFloat(\"y-Coordinate\"), PlayerPrefs.GetFloat(\"y-Coordinate\")),\r\n                0);\r\n\r\n            Vector3 targetVector = targetPosition - transform.position;\r\n            Rigidbody2D rbd = GetComponent<Rigidbody2D>();\r\n            rbd.velocity = new Vector2(targetVector.normalized.x * speed, targetVector.normalized.y * speed);\r\n\r\n            Vector3 len = transform.position - targetPosition;\r\n            float angle = Mathf.Atan2(len.x, len.y) * Mathf.Rad2Deg;\r\n            transform.eulerAngles = new Vector3(0, 0, -angle);\r\n        }\r\n\r\n    }\r\n\r\n    private void OnBecameInvisible()\r\n    {\r\n        Destroy(gameObject);\r\n    }\r\n\r\n    IEnumerator EnemyDie()\r\n    {\r\n        AudioSource audio = GetComponent<AudioSource>();\r\n        audio.Play();\r\n\r\n        Rigidbody2D rbd = GetComponent<Rigidbody2D>();\r\n        rbd.velocity = new Vector2(-rbd.velocity.x, -rbd.velocity.y);\r\n\r\n        CircleCollider2D col = GetComponent<CircleCollider2D>();\r\n        col.enabled = false;\r\n        Animator anim = GetComponent<Animator>();\r\n\r\n        anim.SetTrigger(\"Die\");\r\n        isSlained = false;\r\n\r\n        yield return new WaitForSeconds(0.25f);\r\n\r\n        Destroy(gameObject);\r\n    }\r\n\r\n    private void OnTriggerEnter2D(Collider2D collision)\r\n    {\r\n        if (collision.gameObject.tag == \"Player\")\r\n        {\r\n            GameController controller = GameObject.FindWithTag(\"GameController\").GetComponent<GameController>();\r\n            controller.GameOver();\r\n        }\r\n    }\r\n}";
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < str.Length; i++)
        {
            text.text += str[i];
            if (str[i] == '\n' || str[i] == '\r' || str[i] == '\t')
            {
                continue;
            }
            //audioSource.Play();

            yield return new WaitForSeconds(0.005f);
        }
    }
}
