using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Normandale.DataAccess.SQLControl;
//using Normandale.BusinessRules;
namespace Normandale.DataAccess.Tables
{
    public class StrategicGoals
    {
        public string ConnectionString { get; set; } = "LocalConnectionString";
        public DataTable ReturnedData { get; set; } = new DataTable();
        private DataTable ServerInformation { get; set; } = new DataTable();
        public List<string> ErrorMessages;
        private string _ServerName;
        //private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private String _AdministrationCD;
        private Int32 _GoalSequence;
        private String _StrategicGoal;
        private DateTime? _EffectiveDT;
        private DateTime? _EndDT;
        private DateTime? _LastChgdDT;
        private String _LastChgdBy;
        public String ServerName
        {
            get { return _ServerName; }
        }
        public String AdministrationCD
        {
            get { return _AdministrationCD; }
            set { _AdministrationCD = value; }
        }
        public Int32 GoalSequence
        {
            get { return _GoalSequence; }
            set { _GoalSequence = value; }
        }
        public String StrategicGoal
        {
            get { return _StrategicGoal; }
            set { _StrategicGoal = value; }
        }
        public DateTime? EffectiveDT
        {
            get { return _EffectiveDT; }
            set { _EffectiveDT = value; }
        }
        public DateTime? EndDT
        {
            get { return _EndDT; }
            set { _EndDT = value; }
        }
        public DateTime? LastChgdDT
        {
            get { return _LastChgdDT; }
            set { _LastChgdDT = value; }
        }
        public String LastChgdBy
        {
            get { return _LastChgdBy; }
            set { _LastChgdBy = value; }
        }
        private const string selectClause =
         "SELECT [AdministrationCD] " +
         "     , [GoalSequence] " +
         "     , [StrategicGoal] " +
         "     , [EffectiveDT] " +
         "     , [EndDT] " +
         "     , [LastChgdDT] " +
         "     , [LastChgdBy] " +
         "  FROM Normandale.WorkPlans.StrategicGoals ";
        private const string insertClause =
        " INSERT INTO Normandale.WorkPlans.StrategicGoals " +
         "     ( [AdministrationCD] " +
         "     , [GoalSequence] " +
         "     , [StrategicGoal] " +
         "     , [EffectiveDT] " +
         "     , [EndDT] " +
         "     , [LastChgdDT] " +
         "     , [LastChgdBy] " +
         " )" +
        "VALUES " +
         "     ( @AdministrationCD " +
         "     , @GoalSequence " +
         "     , @StrategicGoal " +
         "     , @EffectiveDT " +
         "     , @EndDT " +
         "     , @LastChgdDT " +
         "     , @LastChgdBy " +
         " )";
        private const string updateClause =
        " UPDATE Normandale.WorkPlans.StrategicGoals " +
         "     SET [AdministrationCD] = @AdministrationCD " +
         "     , [GoalSequence] = @GoalSequence " +
         "     , [StrategicGoal] = @StrategicGoal " +
         "     , [EffectiveDT] = @EffectiveDT " +
         "     , [EndDT] = @EndDT " +
         "     , [LastChgdDT] = @LastChgdDT " +
         "     , [LastChgdBy] = @LastChgdBy " +
         " ";
        private const string deleteClause =
        " DELETE FROM Normandale.WorkPlans.StrategicGoals ";
        private const string orderByClause =
         " ORDER BY [AdministrationCD] " +
         "        , [GoalSequence] " +
         " ";
        private const string whereClause =
        " WHERE 1 = 1 " +
         "   AND [AdministrationCD] = @AdministrationCD " +
         "   AND [GoalSequence] = @GoalSequence " +
         " ";
        public StrategicGoals()
        {
            ErrorMessages = new List<string>();
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppDomain_UnhandledException);
        }
        //static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{
        //Logger.Error(e.ExceptionObject.ToString());
        //}
        public void GetApplicationRows()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            NormandaleQuery.Query = selectClause + WhereClause() + OrderByClause();
            AddParameters();
            NormandaleQuery.RunQuery(ReturnedData);

            if (ReturnedData.Rows.Count == 1)
            {
                LoadObject();
            }
        }
        public void GetServerInformation()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            NormandaleQuery.Query = "SELECT @@servername AS 'Server Name'";
            NormandaleQuery.RunQuery(ServerInformation);
            _ServerName = Convert.ToString(ServerInformation.Rows[0]["Server Name"]);
        }
        private void LoadObject()
        {
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["AdministrationCD"])) AdministrationCD = Convert.ToString(ReturnedData.Rows[0]["AdministrationCD"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["GoalSequence"])) GoalSequence = Convert.ToInt32(ReturnedData.Rows[0]["GoalSequence"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["StrategicGoal"])) StrategicGoal = Convert.ToString(ReturnedData.Rows[0]["StrategicGoal"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["EffectiveDT"])) EffectiveDT = Convert.ToDateTime(ReturnedData.Rows[0]["EffectiveDT"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["EndDT"])) EndDT = Convert.ToDateTime(ReturnedData.Rows[0]["EndDT"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["LastChgdDT"])) LastChgdDT = Convert.ToDateTime(ReturnedData.Rows[0]["LastChgdDT"]);
            if (!Convert.IsDBNull(ReturnedData.Rows[0]["LastChgdBy"])) LastChgdBy = Convert.ToString(ReturnedData.Rows[0]["LastChgdBy"]);
        }
        public int SearchResultsCount()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            int numberOfRows;
            NormandaleQuery.Query = AddCountStart() + selectClause + WhereClause() + AddCountEnd();
            AddParameters();
            numberOfRows = NormandaleQuery.GetCount();
            return numberOfRows;
        }
        public void AddRow()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            //StrategicGoalsBusinessRules.ConnectionString = ConnectionString;
            ErrorMessages.Clear();
            //var result = StrategicGoalsBusinessRules.ValidateRowDoesNotExist(this);
            //if (result.IsSuccessful)
            {
                NormandaleQuery.Query = insertClause;
                AddParameters();
                NormandaleQuery.RunNonQuery();
            }
        }
        public void UpdateRow()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            //StrategicGoalsBusinessRules.ConnectionString = ConnectionString;
            ErrorMessages.Clear();
            //var result = StrategicGoalsBusinessRules.ValidateRowDoesNotExist(this);
            //if (result.IsSuccessful)
            {
                NormandaleQuery.Query = updateClause + whereClause;
                AddParameters();
                NormandaleQuery.RunNonQuery();
            }
        }
        public void DeleteRow()
        {
            NormandaleQuery.ConnectionString = ConnectionString;
            //StrategicGoalsBusinessRules.ConnectionString = ConnectionString;
            ErrorMessages.Clear();
            //var result = StrategicGoalsBusinessRules.ValidateRowDoesNotExist(this);
            //if (result.IsSuccessful)
            {
                NormandaleQuery.Query = deleteClause + whereClause;
                AddParameters();
                NormandaleQuery.RunNonQuery();
            }
        }
        private void AddParameters()
        {
            NormandaleQuery.QueryParameters.Add("@AdministrationCD", new QueryParameterValue { ParameterValue = _AdministrationCD, ParameterType = SqlDbType.VarChar });
            NormandaleQuery.QueryParameters.Add("@GoalSequence", new QueryParameterValue { ParameterValue = _GoalSequence, ParameterType = SqlDbType.Int });
            NormandaleQuery.QueryParameters.Add("@StrategicGoal", new QueryParameterValue { ParameterValue = _StrategicGoal, ParameterType = SqlDbType.VarChar });
            NormandaleQuery.QueryParameters.Add("@EffectiveDT", new QueryParameterValue { ParameterValue = _EffectiveDT, ParameterType = SqlDbType.Date });
            NormandaleQuery.QueryParameters.Add("@EndDT", new QueryParameterValue { ParameterValue = _EndDT, ParameterType = SqlDbType.Date });
            NormandaleQuery.QueryParameters.Add("@LastChgdDT", new QueryParameterValue { ParameterValue = _LastChgdDT, ParameterType = SqlDbType.DateTime });
            NormandaleQuery.QueryParameters.Add("@LastChgdBy", new QueryParameterValue { ParameterValue = _LastChgdBy, ParameterType = SqlDbType.VarChar });
        }
        private string WhereClause()
        {
            string WhereClause = " Where 1=1 ";
            if (!String.IsNullOrEmpty(_AdministrationCD))
            {
                WhereClause += " AND [AdministrationCD] = @AdministrationCD";
            }
            if (_GoalSequence > 0)
            {
                WhereClause += " AND [GoalSequence] = @GoalSequence";
            }
            if (!String.IsNullOrEmpty(_StrategicGoal))
            {
                WhereClause += " AND [StrategicGoal] = @StrategicGoal";
            }
            if (_EffectiveDT.HasValue)
            {
                WhereClause += " AND [EffectiveDT] = @EffectiveDT";
            }
            if (_EndDT.HasValue)
            {
                WhereClause += " AND [EndDT] = @EndDT";
            }
            if (_LastChgdDT.HasValue)
            {
                WhereClause += " AND [LastChgdDT] = @LastChgdDT";
            }
            if (!String.IsNullOrEmpty(_LastChgdBy))
            {
                WhereClause += " AND [LastChgdBy] = @LastChgdBy";
            }
            return WhereClause;
        }
        private string OrderByClause()
        {
            return orderByClause;
        }
        private string AddCountStart()
        {
            return "select count(*) from ( ";
        }
        private string AddCountEnd()
        {
            return " ) as cnt ";
        }
    }
}