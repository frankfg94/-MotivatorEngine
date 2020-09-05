using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;

namespace MotivatorEngine
{
    public abstract class Task
    {
        [JsonIgnore]
        public IMotivatorWindow window;

        //[JsonIgnore]
        //public abstract LicenseType[] CompatibleLicenses { get; set; }

        public abstract TaskInfos Infos {get;set;}

        /// <summary>
        /// Get the score, can be used to measure the total difficulty of the planning
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return this.EstimatedDurationScore + this.EstimatedDifficulty;
        }

        public event EventHandler TaskFinished;
        public event EventHandler TaskStarted;
        public event EventHandler TaskPaused;
        public event EventHandler TaskResumed;
        public event EventHandler TaskDelayed;
        public event EventHandler TaskSkipped;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            // Vérifie si le délégué n'est pas null 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static int taskId = 0;
        /// <summary>
        /// Permet à l'utilisateur de choisir entre la version fermable ou obligatoire de la tâche
        /// </summary>
        public bool PromptDevModeOnStart { get; set; } = true;

        [JsonIgnore]
        /// <summary>
        /// Utilisé pour éviter de recalculer plusieurs fois via la fonction GetEstimatedDuration
        /// </summary>
        /// <remarks>test</remarks>
        protected abstract TimeSpan SecurityDuration { get; }

        /// <summary>
        /// Se produit lorsque la tâche est effectuée , qu'elle soit fermée ou non
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        ///  Optionnel, fenêtre à la fois de tutorial et de configuration pour une fenêtre, retourner Null si rien n'est trouvé
        /// </summary>
        /// <returns></returns>
        protected abstract IConfWindow GetConfWindow();


        //TODO : pourquoi ne pas mettre que des get à l'avenir ici ?
        public bool RequireWebcam { get; set; } = false;
        public bool RequireInternet { get; set; } = false;
        public bool RequireGoodCpu { get; set; } = false;
        public bool RequireMobileApp { get; set; } = false;
        public bool RequireBluetooth { get; set; } = false;

        // Ce morceau de code permet de sérializer un datetime NULLABLE et donc enregistrer ça dans un fichier
        public DateTime? ScheduledDate { get; set; }
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Est également utile pour le FreePlanning
        /// </summary>
        public bool IsFinished
        {
            get => isFinished; set
            {
                isFinished = value;
                if (isFinished)
                {
                    isCurrent = false;
                }
            }
        }

        public bool IsRunning { get; set; }


        public void Start(int configureTimeLimit = 0)
        {
            try
            {

                    StartDirectly();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Erreur pendant le déroulement de la tâche " + "\n" + ex.Message + "\n\n" + ex.StackTrace);
            }

        }

        /// <summary>
        /// Depuis combien de minutes le logiciel a ouvert la fenêtre de configuration/paramétrage/démarrage de la tâche
        /// </summary>
        private int waitConfigureMin = 0;
        /// <summary>
        /// Doit se mettre à vrai lorsque la tâche est utilisée dans un contexte en dehors du planning par exemple, quand on clique sur une tâche depuis l'interface graphique
        /// </summary>
        protected bool startedOutsidePlanning = false;

        /// <summary>
        /// Should not be edited outside a task, use isFinished instead
        /// </summary>
        internal bool isCurrent = false;
        private bool isFinished;

        protected Task()
        {
            taskId++;
        }


        /// <summary>
        /// Obtient la fenêtre associée à la classe dérivée par exemple la méditation
        /// </summary>
        /// 
        protected  IMotivatorWindow GetWindow()
        {
            var confWindow = GetConfWindow();
            if (confWindow != null)
            {
                if(confWindow.ShowAndWaitForChoices())
                {
                    var w = confWindow.GetWindow();
                    confWindow.Close();
                    return w;
                }
            }
            else
            {
                return _GetWindow();
            }
            return null;
        }

        protected abstract IMotivatorWindow _GetWindow();

        /// <summary>
        /// Commence la tâche sans utiliser une fenêtre de configuration/de paramètres/d'options de démarrage, il est important de préciser index et taskCount si nous sommes dans le planning car sinon
        /// l'affichage sera incorrect ( pas d'affichage de la tâche dans l'overlay)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="taskCount"></param>
        public void StartDirectly(int index = 0, int taskCount = 0)
        {
            if(IsFinished)
            {
                Window_Closed(this,null);
            }
            else
            {
                IsRunning = true;
                //if(ScheduledDate.HasValue || startedOutsidePlanning)
                window = GetWindow();
                // if (PromptDevModeOnStart) Utilities.ProposeDevMode();

                /*
                 * Handled in the main app
                if (taskCount > 0)
                    if (this is IPlanningTask)
                        (this as IPlanningTask).DisplayInOverlay(index, taskCount);
                */

                    window.Closed += Window_Closed;
                    window.Show();
                    TaskStarted?.Invoke(this, null);
            }

        }



        protected virtual void Window_Closed(object sender, EventArgs e)
        {
            IsFinished = true;
            IsRunning = false;
            TaskFinished?.Invoke(this, null);
        }



        public void ForceClose()
        {
        }


        [JsonIgnore]
        public abstract TimeSpan EstimatedDuration { get; set; }

        public int EstimatedDurationScore
        {
            get
            {
                var totalMinutes = EstimatedDuration.TotalMinutes;
                if (totalMinutes <= 5)
                {
                    return 1;
                }
                else if(totalMinutes <= 15)
                {
                    return 2;
                }
                else if (totalMinutes <= 30)
                {
                    return 3;
                }
                else if(totalMinutes <= 60)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// Vérifie que la tâche dure suffisamment longtemps pour sauvegarder la progression de la journée pour le planning 
        /// Permet de sauvegarder le planning dès que l'on arrive à une tâche longue, comme ça si le planning plante il ne faudra plus recommencer depuis le début
        /// </summary>
        [JsonIgnore]
        public bool AutoSaveAuthorized { get => EstimatedDuration.TotalHours >= 1; }
        
        /// <summary>
        /// The estimated difficulty is estimated by the task itself, so this property has to be abstract
        /// </summary>
        public abstract int EstimatedDifficulty { get;  set; }

        public void SendToMobile()
        {
        }

        /// <summary>
        /// Indique si la personne a été rapide ou non pour faire la tâche, peut servir à envoyer des mails d'encouragement si la personne n'est pas assez efficace
        /// </summary>
        protected void GetPerformance()
        {
        }

        public override string ToString()
        {
            return "TODO";
        }
    }

}