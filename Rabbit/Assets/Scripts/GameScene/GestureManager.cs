using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene
{
    public class GestureManager : MonoBehaviour
    {
        public GameObject linePrefab = null;
        public GameObject mainPoint = null;
    
        private Vector3 inputPosition = Vector3.zero;
        private List<Vector2> pointList = new List<Vector2>();
        private Rect drawArea;
        private LineRenderer lineRenderer;

        public GestureData nowGesture;
        private Vector2[] sampleArray;
        public bool isDrawable = false;

        private InGameManager inGameManager = null;

        void Awake()
        {
            inGameManager = InGameManager.Getinstance();

            if(inGameManager.gestureManager == null)
            {
                inGameManager.gestureManager = this;
            }

            BoxCollider2D box = transform.Find("DrawBox").GetComponent<BoxCollider2D>();
            
            drawArea = new Rect(new Vector2(box.transform.position.x-box.size.x/2.0f,box.transform.position.y-box.size.y/2.0f),box.size);
        }

        public IEnumerator ShowMainPoint()
        {
            sampleArray = Array.ConvertAll(nowGesture.vecPointArray,data => new Vector2(float.Parse(data.Split('_')[0]),float.Parse(data.Split('_')[1])));
            
            GameObject sampleLine = new GameObject();

            sampleLine.transform.parent = transform;
            sampleLine.name = "Line";
                        
            for(int i=0;i<sampleArray.Length;i++)
            {
                GameObject point;

                if(bool.Parse(nowGesture.vecPointArray[i].Split('_')[2]))
                {
                    //메인 포인트
                    point = Instantiate(mainPoint);
                }
                else
                {
                    //나머지 포인트
                    point = new GameObject();
                }

                point.transform.parent = sampleLine.transform;
                point.transform.position = sampleArray[i];
                point.name = "Point_"+i;
            }

            yield return null;
        }

        void ControlMouse()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Input.mousePosition;
                inputPosition = Camera.main.ScreenToWorldPoint(pos);

                inputPosition.z = 0.0f;

                if (drawArea.Contains(inputPosition))
                {
                    // 처음 누르는 상황
                    if (Input.GetMouseButtonDown(0))
                    {
                        // 기존 선을 삭제한다.
                        if (lineRenderer != null)
                        {
                            Destroy(lineRenderer.gameObject);
                        }

                        GameObject line = Instantiate(linePrefab, transform.position, transform.rotation) as GameObject;
                        lineRenderer = line.GetComponent<LineRenderer>();

                        pointList.Clear();
                    }
                    else
                    {
                        if(pointList.Count == 0 || Vector2.Distance(pointList[pointList.Count-1],inputPosition)>0.01f)
                        {
                            pointList.Add(inputPosition);

                            lineRenderer.positionCount = pointList.Count;
                            lineRenderer.SetPositions(Array.ConvertAll(pointList.ToArray(),vec2 => new Vector3(vec2.x,vec2.y,0.0f)));
                        }
                    }
                }
            }
        }

        void Update()
        {
            if(isDrawable)
            {
                ControlMouse();
            }
        }

        int Classify(Vector2[] candidate)
	    {            
            float distance = GreedyCloudMatch(SamplingGesture(candidate),SamplingGesture(sampleArray));
            
            return (int)((Mathf.Max((distance-2.0f)/-2.0f,0.0f))*100.0f);
	    }

        float GreedyCloudMatch(Vector2[] _pointArrayA,Vector2[] _pointArrayB)
	    {
            int length = _pointArrayA.Length;
            int step = (int)Math.Floor(Math.Sqrt(length));
            float distance = float.MaxValue;

            for (int i=0;i<length;i+=step)
            {
                float distanceA = CloudDistance(_pointArrayA, _pointArrayB,i);
                float distanceB = CloudDistance(_pointArrayB, _pointArrayA,i);

                distance = Mathf.Min(distance, Mathf.Min(distanceA,distanceB));
            }

            return distance;
        }

        float CloudDistance(Vector2[] _pointArrayA,Vector2[] _pointArrayB,int _startIndex)
	    {
		    int length = _pointArrayA.Length;
            bool[] matched = new bool[length];

            Array.Clear(matched,0,length);

            float sum = 0;
            int i = _startIndex;

		    do
		    {
			    int index=-1;
			    float minDistance = float.MaxValue;

			    for(int j=0;j<length; j++)
                {
                    if (!matched[j])
                    {
                        float distance = Vector2.SqrMagnitude(_pointArrayA[i]-_pointArrayB[j]);

                        if (distance<minDistance)
                        {
                            minDistance = distance;
                            index = j;
                        }
                    }
                }

			    matched[index] = true;

                float weight = 1.0f-((i-_startIndex+length)%length)/(float)length;

                sum += weight*minDistance;

                i = (i+1)%length;

            } while (i != _startIndex);

		    return sum;
	    }

        public int Recognize()
        {        
            float dis = -1.0f;
            Vector3 maxPos = Vector3.zero;
            int percent = 0;
        
            //직선의 점이 4개 이하라면 무시한다.
            if(pointList.Count<4)
            {
                percent = 0;
            }
            else
            {
                foreach (Vector2 pos in pointList)
                {
                    dis = Mathf.Max(dis,Vector2.Distance(pointList[0],pos));
                }
        
                if(dis<0.2f)
                {
                    percent = 0;
                }
                else
                {
                    percent = Classify(pointList.ToArray());
                }
            }


            return percent;
        }
        
        Vector2[] Scale(Vector2[] _pointArray)
	    {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i=0;i<_pointArray.Length; i++)
		    {
                minX = Mathf.Min(minX,_pointArray[i].x);
                minY = Mathf.Min(minY,_pointArray[i].y);
                maxX = Mathf.Max(maxX,_pointArray[i].x);
                maxY = Mathf.Max(maxY,_pointArray[i].y);
		    }

            float scale = Mathf.Max(maxX-minX,maxY-minY);

            return Array.ConvertAll(_pointArray,vec2 =>new Vector2(vec2.x-minX,vec2.y-minY)/scale);
	    }

	    public Vector2[] SamplingGesture(Vector2[] _pointArray)
	    {
            // 포인트 배열의 크기를 줄인다.
            Vector2[] SPointArray = Scale(_pointArray);

            // 표준화 표본 개수
            const int _index = 32;

            Vector2 centroid = new Vector2(SPointArray.Sum(vec2=>vec2.x),SPointArray.Sum(vec2=>vec2.y))/SPointArray.Length;

            Vector2[] TPointArray = Array.ConvertAll(SPointArray,vec2=>vec2-centroid);
        
            Vector2[] newPoints = new Vector2[_index];

		    newPoints[0] = TPointArray[0];

		    int numPoints = 1;
			
		    float length = PathLength(TPointArray)/(_index-1); 
		    float D = 0;

		    for (int i=1;i<TPointArray.Length;i++)
		    {
			    float d = Vector2.Distance(TPointArray[i-1],TPointArray[i]);  //두점 사이의 거리

                if (D+d>=length)
			    {
                    Vector2 firstPoint = TPointArray[i-1];

				    while (D+d>=length)
				    {
					    float t = Mathf.Min(Mathf.Max((length - D) / d, 0.0f), 1.0f);

					    if (float.IsNaN(t)) t = 0.5f;

					    newPoints[numPoints++] = Vector2.Lerp(firstPoint, TPointArray[i], t);

                        d = D + d - length;
					    D = 0;
					    firstPoint = newPoints[numPoints - 1];
				    }

				    D = d;
			    }
			    else
                    D += d;
		    }
			
		    if (numPoints == _index-1) 
			    newPoints[numPoints++] = TPointArray[TPointArray.Length-1];

            return newPoints;
	    }

	    private float PathLength(Vector2[] _pointArray)
	    {
		    float length = 0;

		    for (int i=1;i<_pointArray.Length; i++)
            {
                length += Vector2.Distance(_pointArray[i-1],_pointArray[i]);
            }

		    return length;
	    }

    }
}