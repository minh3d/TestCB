using UnityEngine;
using System.Collections;
using DG.Tweening;


public class TAPPFloating : MonoBehaviour
{

    public Transform float1;
    public Transform float2;

    public Transform btt1;
    public Transform btt2;
    public Transform btt3;
    public Transform btt4;

    private bool isDrag = false;
    private bool isOpen = false;
    private bool isLeft = false;

    // Use this for initialization
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        transform.DOMoveX(float1.position.x, 0.5f);
        isLeft = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BeginDrag()
    {
        isDrag = true;
    }
    public void OnDrag() {
        transform.position = Input.mousePosition;
    }

    public void EndDrag()
    {
        isDrag = false;

        //move to border
        if (Mathf.Abs(transform.position.x - float1.position.x) > Mathf.Abs(transform.position.x - float2.position.x))
        {
            transform.DOMoveX(float2.position.x, 0.5f);
            isLeft = false;
        } else
        {
            transform.DOMoveX(float1.position.x, 0.5f);
            isLeft = true;
        }

        if (isOpen)
        {
            close();
        }
    }


    public void Click()
    {
        if (!isDrag)
        {
            if (isOpen)
            {
                close();
            }
            else
            {
                open();
            }
        }
    }

    void close()
    {
        isOpen = false;
        btt1.DOLocalMoveX(0, 0.5f);
        btt2.DOLocalMoveX(0, 0.5f);
        btt3.DOLocalMoveX(0, 0.5f);
        btt4.DOLocalMoveX(0, 0.5f);
        if (isLeft)
        {
            transform.DOMoveX(transform.position.x - 30, 0.5f);
        }
        else
        {
            transform.DOMoveX(transform.position.x + 30, 0.5f);
        }
    }

    void open()
    {
        isOpen = true;
        if (isLeft)
        {
            transform.DOMoveX(transform.position.x + 30, 0.5f);
            btt1.DOLocalMoveX(27, 0.5f);
            btt2.DOLocalMoveX(54, 0.5f);
            btt3.DOLocalMoveX(81, 0.5f);
            btt4.DOLocalMoveX(108, 0.5f);
        } else {
            transform.DOMoveX(transform.position.x - 30, 0.5f);
            btt1.DOLocalMoveX(-27, 0.5f);
            btt2.DOLocalMoveX(-54, 0.5f);
            btt3.DOLocalMoveX(-81, 0.5f);
            btt4.DOLocalMoveX(-108, 0.5f);

        }

    }
}
