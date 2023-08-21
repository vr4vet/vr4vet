/* Developer: Abbas Jafari
 * Ask your questions by email: a85jafari@gmail.com
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Task
{
    /// <summary>
    /// This class will control the navigation system and Navmesh
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        public static NavigationManager navigationManager;

        [HideInInspector] private Button activeButton;
        [HideInInspector] public GameObject target = null;

        [Header("GameObjects")]
        public GameObject player;

        [Space(4)]
        public GameObject Arrow;

        //     public GameObject TaskListContentView;

        [Header("Sounds")]
        public AudioClip activated;

        public AudioClip deactivated;
        public AudioClip prerequisites;

        [Header("Other Settings")]
        [Range(0f, 1f)]
        public float navArrowSpeed = 0.7f;

        private Vector3 curPos;
        private Vector3 lastPos;

        private GameObject newArrow;

        private bool TaskIsActive;

        private List<GameObject> CurrentArrows = new List<GameObject>();

        private void Start()
        {
            if (navigationManager == null)
                navigationManager = this;
            else if (navigationManager != this)
                Destroy(gameObject);

            if (player) createNavigation();
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");
                if (!player)
                {
                    Debug.LogError("Assign the player under NavigationManager or with Player tag, " +
                        "otherwise navigation system will not working");
                }
                else
                {
                    createNavigation();
                }
            }
        }

        /// <summary>
        /// Start createing the navigation and the arrows
        /// </summary>
        private void createNavigation()
        {
            if (target && target.activeInHierarchy)
            {
                //modified the script so it won't foolow the Y axis
                newArrow = Instantiate(Arrow, new Vector3(player.transform.position.x, 0, player.transform.position.z), player.transform.rotation);
                if (newArrow.GetComponent<NavMeshAgent>().velocity != Vector3.zero)
                    newArrow.transform.rotation = Quaternion.LookRotation(newArrow.GetComponent<NavMeshAgent>().velocity, Vector3.up);
                CurrentArrows.Add(newArrow);
            }

            Invoke("createNavigation", 1 - navArrowSpeed);
        }

        private void LateUpdate()
        {
            if (player == null) return;

            //check if playter moves
            curPos = player.transform.position;
            if (curPos != lastPos)
            {
                foreach (GameObject arrow in CurrentArrows)
                {
                    Destroy(arrow);
                }

                CurrentArrows.Clear();
            }
            lastPos = curPos;
        }

        /// <summary>
        /// Set a new target for navigation system
        /// </summary>
        /// <param name="thisOppgave"></param>
        /// <param name="button"></param>
        public void SetTarget(Task thisOppgave)
        {
            //task is allready done
            if (thisOppgave.Compleated())
            {
                //taskState.PlayAudio(TaskIsDoneAllredy);
                target = null;
                ResetNavigation();
                return;
            }
            else
            {
                PlayAudio(prerequisites);
                ResetNavigation();
                return;
            }

            //****Task activating*****

            //if task need to active pathfinding
            if (thisOppgave.target && thisOppgave.target.activeInHierarchy)
            {
                /*if the button is not assigned and you set the target with code,
                 just set null for last parameter in SetTarget method. a fake button will
                 prevent nullreference error*/
                if (!activeButton)
                {
                    activeButton = new GameObject("fake button").AddComponent<Button>();
                    activeButton.gameObject.AddComponent<Image>();
                }

                //if task is not don yet
                if (!thisOppgave.Compleated())
                {
                    //deactive
                    if (TaskIsActive)
                    {
                        //todeactive the active task
                        if (activeButton.GetComponent<Image>().color == Color.yellow)
                        {
                            PlayAudio(deactivated);
                            target = null;
                            ResetNavigation();
                        }
                        else
                        {
                            //If a task is active and another task will be activated by script
                            ResetNavigation();
                            SetTarget(thisOppgave);
                            return;
                        }
                    }
                    //active
                    else
                    {
                        PlayAudio(activated);
                        target = thisOppgave.target;
                        ResetNavigation(); // this will make TaskIsActive false, so change it to true again if needed
                        TaskIsActive = true;

                        //should be after ResetNavigation()
                        activeButton.GetComponent<Image>().color = Color.yellow;
                    }
                }
            }
            else
            {
                target = null;
                ResetNavigation();
                return;
            }
        }

        /// <summary>
        /// REmove all arrows that are created
        /// </summary>
        /// <param name="arrow"></param>
        public void removeArrowFromList(GameObject arrow)
        {
            CurrentArrows.Remove(arrow);
        }

        /// <summary>
        /// Reset navigation system
        /// </summary>
        public void ResetNavigation()
        {
            TaskIsActive = false;

            //reset all buttons color
            /*(
              foreach (Button btn in TaskListContentView.GetComponentsInChildren<Button>())
              {
                  Color btnWhite = new Color(20,80,140,100);
                  btn.GetComponent<Image>().color = btnWhite;
              }
            */
            foreach (GameObject arrow in CurrentArrows)
                Destroy(arrow);

            //clear the list of all arrows
            CurrentArrows.Clear();
        }

        /// <summary>
        /// Play a sound clip
        /// </summary>
        /// <param name="clipp"></param>
        public void PlayAudio(AudioClip clipp)
        {
            //if the gameobject has audiosource
            if (TryGetComponent<AudioSource>(out AudioSource audioSource))
            {
                audioSource.Stop();
                audioSource.PlayOneShot(clipp);
                return;
            }

            //otherwise create audiosource
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.PlayOneShot(clipp);
        }
    }
}