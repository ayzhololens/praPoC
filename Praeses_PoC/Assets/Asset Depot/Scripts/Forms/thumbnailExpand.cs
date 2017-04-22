using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class thumbnailExpand : MonoBehaviour {
        public float ExpandMult;
        public float ShrinkMult;
        Vector3 startScale;
        Vector3 simpleStartScale;
        Vector3 photoStartScale;
        Vector3 videoStartScale;
        public formFieldController linkedField;
        public float breakOutScale;
        public Transform breakOutPos;
        Vector3 startPos;
        bool brokeOut;

        // Use this for initialization
        void Start()
        {
            linkedField = GetComponent<commentContents>().linkedComponent.GetComponent<formFieldController>();
            if (linkedField.GetComponent<formFieldController>() != null)
            {
                simpleStartScale = linkedField.simpleNotePrefab.transform.localScale;
                photoStartScale = linkedField.photoThumbPrefab.transform.localScale;
                videoStartScale = linkedField.videoThumbPrefab.transform.localScale;

            }
            for (int i = 0; i<transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i).gameObject.name == "breakOut")
                {
                    breakOutPos = transform.parent.GetChild(i);
                }
            }
            startScale = transform.localScale;
            startPos = transform.localPosition;

        }

        // Update is called once per frame
        void Update() {

        }

        public void expandThumbnail()
        {
            if (!brokeOut)
            {

                transform.localScale = new Vector3 (transform.localScale.x * ExpandMult, transform.localScale.y* ExpandMult, transform.localScale.z);
                shrinkThumbnails();
            }

        }

        void shrinkThumbnails()
        {


            if(linkedField.activeSimpleNotes.Count != 0)
            {
                for (int i = 0; i < linkedField.activeSimpleNotes.Count; i++)
                {
                    if (linkedField.activeSimpleNotes[i] != this.gameObject)
                    {
                        simpleStartScale = linkedField.activeSimpleNotes[i].transform.localScale;
                        linkedField.activeSimpleNotes[i].transform.localScale = linkedField.activeSimpleNotes[i].transform.localScale * ShrinkMult;

                    }

                }
            }


            for (int i = 0; i < linkedField.activePhotos.Count; i++)
            {
                if (linkedField.activePhotos[i] != this.gameObject)
                {
                    photoStartScale = linkedField.activePhotos[i].transform.localScale;
                    linkedField.activePhotos[i].transform.localScale = linkedField.activePhotos[i].transform.localScale * ShrinkMult;
                }

            }

            for (int i = 0; i < linkedField.activeVideos.Count; i++)
            {
                if (linkedField.activeVideos[i] != this.gameObject)
                {
                    videoStartScale = linkedField.activeVideos[i].transform.localScale;
                    linkedField.activeVideos[i].transform.localScale = linkedField.activeVideos[i].transform.localScale * ShrinkMult;

                }

            }

        }

        public void resetThumbs()
        {
            if (!brokeOut)
            {
                if (linkedField.activeSimpleNotes.Count != 0)
                {
                    for (int i = 0; i < linkedField.activeSimpleNotes.Count; i++)
                    {
                        if (!linkedField.activeSimpleNotes[i].activeSelf)
                        {
                            linkedField.activeSimpleNotes[i].SetActive(true);
                        }
                        linkedField.activeSimpleNotes[i].transform.localScale = simpleStartScale;
                    }

                }


                for (int i = 0; i < linkedField.activePhotos.Count; i++)
                {
                    if (!linkedField.activePhotos[i].activeSelf)
                    {
                        linkedField.activePhotos[i].SetActive(true);
                    }
                    linkedField.activePhotos[i].transform.localScale = photoStartScale;

                }

                for (int i = 0; i < linkedField.activeVideos.Count; i++)
                {
                    if (!linkedField.activeVideos[i].activeSelf)
                    {
                        linkedField.activeVideos[i].SetActive(true);
                        if (linkedField.activeVideos[i].GetComponent<commentContents>().playIcon.activeSelf)
                        {
                            linkedField.activeVideos[i].GetComponent<commentContents>().playIcon.SetActive(false);
                        }
                        
                    }
                    linkedField.activeVideos[i].transform.localScale = videoStartScale;

                }
                transform.localPosition = startPos;

                GetComponent<commentContents>().commentMetaUser.gameObject.SetActive(false);
            }   

        }

        public void breakOutThumb()
        {
            if (!brokeOut)
            {
                brokeOut = true;


                if (linkedField.activeSimpleNotes.Count != 0)
                {
                    for (int i = 0; i < linkedField.activeSimpleNotes.Count; i++)
                    {
                        if (linkedField.activeSimpleNotes[i] != this.gameObject)
                        {
                            linkedField.activeSimpleNotes[i].SetActive(false);

                        }

                    }

                }



                for (int i = 0; i < linkedField.activePhotos.Count; i++)
                {
                    if (linkedField.activePhotos[i] != this.gameObject)
                    {
                        linkedField.activePhotos[i].SetActive(false);
                    }

                }

                for (int i = 0; i < linkedField.activeVideos.Count; i++)
                {
                    if (linkedField.activeVideos[i] != this.gameObject)
                    {
                        linkedField.activeVideos[i].SetActive(false);
                    }

                }

                transform.localScale = transform.localScale * breakOutScale;
                GetComponent<commentContents>().commentMetaUser.gameObject.SetActive(true);

                if (GetComponent<commentContents>().isVideo)
                {
                    GetComponent<commentContents>().playIcon.SetActive(true);
                }
                transform.localPosition = breakOutPos.localPosition;
            }

        }

        public void closeBreakout()
        {

            brokeOut = false;
            if (GetComponent<commentContents>().isVideo)
            {
                GetComponent<commentContents>().playIcon.SetActive(false);
            }
            resetThumbs();
        }
    }
}