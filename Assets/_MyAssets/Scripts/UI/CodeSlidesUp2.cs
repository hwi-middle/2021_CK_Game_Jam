using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeSlidesUp2 : MonoBehaviour
{
    private Text text;
    private string str = "#include <bits/stdc++.h>\r\n#define left _left\r\n#define right _right\r\n\r\nusing namespace std;\r\n\r\nstruct Node\r\n{\r\n\tint left, right;\r\n\tint order, depth;\r\n};\r\n\r\nNode a[10001];\r\nint left[10001];\r\nint right[10001];\r\nint cnt[10001];\r\nint order = 0;\r\n\r\nvoid inorder(int node, int depth)\r\n{\r\n\tif (node == -1) return;\r\n\tinorder(a[node].left, depth + 1);\r\n\ta[node].order = ++order;\r\n\ta[node].depth = depth;\r\n\tinorder(a[node].right, depth + 1);\r\n}\r\n\r\nint main() {\r\n\tios::sync_with_stdio(0);\r\n\tcin.tie(0);\r\n\r\n\tint n;\r\n\tcin >> n;\r\n\tfor (int i = 0; i < n; i++)\r\n\t{\r\n\t\tint x, y, z;\r\n\t\tcin >> x >> y >> z;\r\n\t\ta[x].left = y;\r\n\t\ta[x].right = z;\r\n\t\tif (y != -1) cnt[y] += 1;\r\n\t\tif (z != -1) cnt[z] += 1;\r\n\t}\r\n\r\n\tint root = 0;\r\n\tfor (int i = 1; i <= n; i++)\r\n\t{\r\n\t\tif (cnt[i] == 0)\r\n\t\t{\r\n\t\t\troot = i;\r\n\t\t}\r\n\t}\r\n\r\n\tinorder(root, 1);\r\n\tint maxdepth = 0;\r\n\r\n\tfor (int i = 1; i <= n; i++)\r\n\t{\r\n\t\tint depth = a[i].depth;\r\n\t\tint order = a[i].order;\r\n\t\tif (left[depth] == 0)\r\n\t\t{\r\n\t\t\tleft[depth] = order;\r\n\t\t}\r\n\t\telse\r\n\t\t{\r\n\t\t\tleft[depth] = min(left[depth], order);\r\n\t\t}\r\n\t\tright[depth] = max(right[depth], order);\r\n\t\tmaxdepth = max(maxdepth, depth);\r\n\t}\r\n\r\n\tint ans = 0;\r\n\tint ans_level = 0;\r\n\tfor (int i = 1; i <= maxdepth; i++)\r\n\t{\r\n\t\tif (ans < right[i] - left[i] + 1)\r\n\t\t{\r\n\t\t\tans = right[i] - left[i] + 1;\r\n\t\t\tans_level = i;\r\n\t\t}\r\n\t}\r\n\r\n\tcout << ans_level << ' ' << ans;\r\n\r\n\treturn 0;\r\n}";
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
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < str.Length; i++)
        {
            text.text += str[i];
            if (str[i] == '\n' || str[i] == '\r' || str[i] == '\t')
            {
                continue;
            }
            //audioSource.Play();

            yield return new WaitForSeconds(0.01f);
        }
    }
}
