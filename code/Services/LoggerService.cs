using System.Security.Claims;
using code.Models;

namespace code.Services
{
    public class LoggerService 
    {
        private static string CONF_FILE_NAME = "vlakyLog";
        private static string CONF_FILE_SUFIX = ".txt";

        private static string CONF_COLUMN_SEPARATOR = "#";
        private static string CONF_INCOLUMN_SEPARETOR = "|";
        private static string CONF_ARROW = "->";
        private static string CONF_LABEL = ":";

        private static string CONF_LOGIN = "log_in";

        private static string CONF_CHANGE = "change";
        private static string CONF_ACC_NEW = "new";
        private static string CONF_ACC_DELETE = "delete";
        private static string CONF_MAIL = "mail";
        private static string CONF_PASS = "password";
        private static string CONF_PRIVILEGES = "privileges";

        private static string CONF_TEMPLATE = "template";
        private static string CONF_TEMPLATE_NEW = "new";
        private static string CONF_TEMPLATE_DELETE = "delete";
        private static string CONF_TEMPLATE_CHANGE = "change";

        private static string CONF_TRAIN = "train";
        private static string CONF_TRAIN_NEW = "new";
        private static string CONF_TRAIN_ADD = "add";
        private static string CONF_TRAIN_CHANGE = "change";

        private static string CONF_WAGON = "wagon";
        private static string CONF_WAGON_NEW = "new";
        private static string CONF_WAGON_DELETE = "delete";

        private static string CONF_COMM = "comm";
        private static string CONF_COMM_BOARD = "board";
        private static string CONF_COMM_TRAIN = "train";
        private static string CONF_COMM_WAGON = "wagon";
        private static string CONF_COMM_NEW = "new";
        private static string CONF_COMM_DELETE = "delete";
        private static string CONF_COMM_CHANGE = "change";

        private int year = 0;
        private string? fileName;

        public LoggerService() 
        {
            reconf(DateTime.Now);
        }

        public static void configureService(ConfigurationManager config) 
        {
            var conf = config.GetSection("FileLoggingOptions");
            CONF_FILE_NAME = conf["FileName"] ?? CONF_FILE_NAME;
            CONF_FILE_SUFIX = conf["FileSufix"] ?? CONF_FILE_SUFIX;
            CONF_COLUMN_SEPARATOR = conf["ColumnSeparator"] ?? CONF_COLUMN_SEPARATOR;
            CONF_INCOLUMN_SEPARETOR = conf["InColumnSeparator"] ?? CONF_INCOLUMN_SEPARETOR;
            CONF_ARROW = conf["ArrowSeparator"] ?? CONF_ARROW;
            CONF_LABEL = conf["LabelSeparator"] ?? CONF_LABEL;

            var loginConf = conf.GetSection("LoginOptions");
            CONF_LOGIN = loginConf["PrimaryLabel"] ?? CONF_LOGIN;

            var changeConf = conf.GetSection("ChangeOptions");
            CONF_CHANGE = changeConf["PrimaryLabel"] ?? CONF_CHANGE;
            CONF_ACC_NEW = changeConf["SecondaryLabelNew"] ?? CONF_ACC_NEW;
            CONF_ACC_DELETE = changeConf["SecondaryLabelDelete"] ?? CONF_ACC_DELETE;
            CONF_MAIL = changeConf["SecondaryLabelMail"] ?? CONF_MAIL;
            CONF_PASS = changeConf["SecondaryLabelPassword"] ?? CONF_PASS;
            CONF_PRIVILEGES = changeConf["SecondaryLabelPrivileges"] ?? CONF_PRIVILEGES;

            var tempConf = conf.GetSection("TemplateOptions");
            CONF_TEMPLATE = tempConf["PrimaryLabel"] ?? CONF_TEMPLATE;
            CONF_TEMPLATE_NEW = tempConf["SecondaryLabelNew"] ?? CONF_TEMPLATE_NEW;
            CONF_TEMPLATE_CHANGE = tempConf["SecondaryLabelChange"] ?? CONF_TEMPLATE_CHANGE;
            CONF_TEMPLATE_DELETE = tempConf["SecondaryLabelDelete"] ?? CONF_TEMPLATE_DELETE;

            var trainConf = conf.GetSection("TrainOptions");
            CONF_TRAIN = trainConf["PrimaryLabel"] ?? CONF_TRAIN;
            CONF_TRAIN_NEW = trainConf["SecondaryLabelNew"] ?? CONF_TRAIN_NEW;
            CONF_TRAIN_ADD = trainConf["SecondaryLabelAdd"] ?? CONF_TRAIN_ADD;
            CONF_TRAIN_CHANGE = trainConf["SecondaryLabelChange"] ?? CONF_TRAIN_CHANGE;

            var wagonConf = conf.GetSection("WagonOptions");
            CONF_WAGON = wagonConf["PrimaryLabel"] ?? CONF_WAGON;
            CONF_WAGON_NEW = wagonConf["SecondaryLabelNew"] ?? CONF_WAGON_NEW;
            CONF_WAGON_DELETE = wagonConf["SecondaryLabelDelete"] ?? CONF_WAGON_DELETE;

            var commConf = conf.GetSection("CommentsOptions");
            CONF_COMM = commConf["PrimaryLabel"] ?? CONF_COMM;
        }

        private void reconf(DateTime now) 
        {
            year = now.Year;
            fileName = CONF_FILE_NAME + year + CONF_FILE_SUFIX;
        }

        private void writeToLog(HttpContext context, string str)
        {
            string name = context.User.FindFirstValue(ClaimTypes.Name) != null ? 
                context.User.FindFirstValue(ClaimTypes.Name).ToString() : "unknown";
            writeToLog(name, str);
        }

        private void writeToLog(string name, string str)
        {
            DateTime now = DateTime.Now;
            if (year < now.Year) {
                reconf(now);
            }

            if (!File.Exists(fileName))
            {
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(now.ToString() + 
                                CONF_COLUMN_SEPARATOR + 
                                name +
                                str);
                }	
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine(now.ToString() + 
                                CONF_COLUMN_SEPARATOR + 
                                name +
                                str);
                }
            }
        }

        public void writeLogIn(string name)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_LOGIN;

            writeToLog(name, str);
        }

        public void writeUserChange(HttpContext context, string str)
        {
            str = CONF_COLUMN_SEPARATOR + 
            CONF_CHANGE +
            CONF_INCOLUMN_SEPARETOR + 
            str;

            writeToLog(context, str);
        }

        public void writeUserNew(HttpContext context, Account acc)
        {
            string str = acc.Name +
            CONF_COLUMN_SEPARATOR + 
            CONF_ACC_NEW;

            writeUserChange(context, str);
        }

        public void writeUserDelete(HttpContext context, Account acc)
        {
            string str = acc.Name +
            CONF_COLUMN_SEPARATOR + 
            CONF_ACC_DELETE;

            writeUserChange(context, str);
        }

        public void writeUserMailChange(HttpContext context, Account oldAcc, Account newAcc)
        {
            string str = oldAcc.Name +
            CONF_COLUMN_SEPARATOR + 
            CONF_MAIL +
            CONF_INCOLUMN_SEPARETOR +
            oldAcc.Mail + 
            CONF_ARROW +
            newAcc.Mail;

            writeUserChange(context, str);
        }

        public void writeUserPassChange(HttpContext context, Account acc)
        {
            string str = acc.Name +
            CONF_COLUMN_SEPARATOR + 
            CONF_PASS;

            writeUserChange(context, str);
        }

        public void writeUserPrivilegesChange(HttpContext context, Account oldAcc, Account newAcc)
        {
            string str = oldAcc.Name +
            CONF_COLUMN_SEPARATOR + 
            CONF_PRIVILEGES +
            CONF_INCOLUMN_SEPARETOR +
            newAcc.Privileges.ToString() + 
            CONF_ARROW +
            oldAcc.Privileges.ToString();

            writeUserChange(context, str);
        }

        private void writeTemplate(HttpContext context, string str)
        {
            str = CONF_COLUMN_SEPARATOR + 
            CONF_TEMPLATE + 
            CONF_INCOLUMN_SEPARETOR + 
            str;

            writeToLog(context, str);
        }

        public void writeTemplateNew(HttpContext context, TrainTemplate template)
        {
            string str = template.Name + 
            CONF_COLUMN_SEPARATOR + 
            CONF_TEMPLATE_NEW;

            writeTemplate(context, str);
        }

        public void writeTemplateChange(HttpContext context, TrainTemplate template)
        {
            string str = template.Name + 
            CONF_COLUMN_SEPARATOR + 
            CONF_TEMPLATE_CHANGE;

            writeTemplate(context, str);
        }

        public void writeTemplateDelete(HttpContext context, TrainTemplate template)
        {
            string str = template.Name + 
            CONF_COLUMN_SEPARATOR + 
            CONF_TEMPLATE_DELETE;

            writeTemplate(context, str);
        }

        private void writeTrain(HttpContext context, string str)
        {
            str = CONF_COLUMN_SEPARATOR + 
            CONF_TRAIN + 
            str;

            writeToLog(context, str);
        }

        private void writeTrain(HttpContext context, Train train, string str)
        {
            str = CONF_INCOLUMN_SEPARETOR + 
            train.Id + 
            str;

            writeTrain(context, str);
        }

        public void writeTrainNew(HttpContext context, Train train)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_TRAIN_NEW;

            writeTrain(context, train, str);
        }

        public void writeTrainAdd(HttpContext context, Train train, Wagon wagon)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_TRAIN_ADD + 
            CONF_INCOLUMN_SEPARETOR + 
            wagon.TrainId + 
            CONF_INCOLUMN_SEPARETOR + 
            wagon.NOrder;

            writeTrain(context, train, str);
        }

        private string trainChange(string label, string str1, string str2)
        {
            return CONF_COLUMN_SEPARATOR + 
                CONF_TRAIN_CHANGE + 
                CONF_INCOLUMN_SEPARETOR + 
                label + 
                CONF_LABEL + 
                str1 + 
                CONF_ARROW + 
                str2;
        }

        public void writeTrainChange(HttpContext context, Train oldTrain, Train newTrain)
        {
            if (oldTrain.Name != newTrain.Name) {
                writeTrain(context, oldTrain, 
                    trainChange("Name", oldTrain.Name, newTrain.Name));
            }

            if (oldTrain.Destination != newTrain.Destination) {
                writeTrain(context, oldTrain, 
                    trainChange("Destination", oldTrain.Destination, newTrain.Destination));
            }

            if (oldTrain.Status != newTrain.Status) {
                writeTrain(context, oldTrain, 
                    trainChange("Status", oldTrain.Status.ToString(), newTrain.Status.ToString()));
            }

            if (oldTrain.Date != newTrain.Date) {
                writeTrain(context, oldTrain, 
                    trainChange("Date", oldTrain.Date.ToString(), newTrain.Date.ToString()));
            }

            if (oldTrain.Coll != newTrain.Coll) {
                writeTrain(context, oldTrain, 
                    trainChange("Coll", oldTrain.Coll.ToString(), newTrain.Coll.ToString()));
            }

            if (oldTrain.MaxLength != newTrain.MaxLength) {
                writeTrain(context, oldTrain, 
                    trainChange("MaxLength", oldTrain.MaxLength.ToString(), newTrain.MaxLength.ToString()));
            }

            if (oldTrain.Lenght != newTrain.Lenght) {
                writeTrain(context, oldTrain, 
                    trainChange("Lenght", oldTrain.Lenght.ToString(), newTrain.Lenght.ToString()));
            }
        }

        private void writeWagon(HttpContext context, string str)
        {
            str = CONF_COLUMN_SEPARATOR + 
            CONF_WAGON + 
            str;

            writeToLog(context, str);
        }

        private void writeWagon(HttpContext context, string str, Wagon wagon)
        {
            str = CONF_INCOLUMN_SEPARETOR + 
            wagon.TrainId + 
            CONF_INCOLUMN_SEPARETOR + 
            wagon.NOrder + 
            str;

            writeWagon(context, str);
        }

        public void writeWagonNew(HttpContext context, Wagon wagon)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_WAGON_NEW;

            writeWagon(context, str, wagon);
        }

        public void writeWagonDelete(HttpContext context, Wagon wagon)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_WAGON_DELETE;

            writeWagon(context, str, wagon);
        }

        public void writeWagonChange(HttpContext context, Wagon oldWagon, Wagon newWagon)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            oldWagon.State + 
            CONF_ARROW + 
            newWagon.State;

            writeWagon(context, str, oldWagon);
        }

        private void writeComm(HttpContext context)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_COMM;

            writeToLog(context, str);
        }

        private void writeComm(HttpContext context, Note note, string str)
        {
            string label;
            int id;
            if (note is BlackBoardNote) {
                label = CONF_COMM_BOARD;
                id = ((BlackBoardNote) note).Id;
            } else if (note is TrainNote) {
                label = CONF_COMM_TRAIN;
                id = ((TrainNote) note).TrainId;
            } else {
                label = CONF_COMM_WAGON;
                id = ((WagonNote) note).WagonId;
            }

            str = CONF_COLUMN_SEPARATOR + 
            CONF_COMM + 
            CONF_INCOLUMN_SEPARETOR +
            label + 
            CONF_INCOLUMN_SEPARETOR +
            id;

            writeToLog(context, str);
        }

        public void writeCommNew(HttpContext context, Note note)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_COMM_NEW;

            writeComm(context, note, str);
        }

        public void writeCommDelete(HttpContext context, Note note)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_COMM_DELETE;

            writeComm(context, note, str);
        }

        public void writeCommChange(HttpContext context, Note note)
        {
            string str = CONF_COLUMN_SEPARATOR + 
            CONF_COMM_CHANGE;

            writeComm(context, note, str);
        }
    }
}