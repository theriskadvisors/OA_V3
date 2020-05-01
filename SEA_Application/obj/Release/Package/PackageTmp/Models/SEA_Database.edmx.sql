
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/09/2019 18:23:30
-- Generated from EDMX file: C:\Users\mrasa\Documents\GitHub\Version1_SEA\SEA_Application\Models\SEA_Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NGS_Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Event__UserId__52442E1F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK__Event__UserId__52442E1F];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvanceSalaryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeAdvanceSalary] DROP CONSTRAINT [FK_AdvanceSalaryUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Announcement_Subject_ToAnnouncement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAnnouncement_Subject] DROP CONSTRAINT [FK_Announcement_Subject_ToAnnouncement];
GO
IF OBJECT_ID(N'[dbo].[FK_Announcement_Subject_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAnnouncement_Subject] DROP CONSTRAINT [FK_Announcement_Subject_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAssessment_Question_ToQuestionCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAssessment_Question] DROP CONSTRAINT [FK_AspNetAssessment_Question_ToQuestionCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAssessment_Question_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAssessment_Question] DROP CONSTRAINT [FK_AspNetAssessment_Question_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAssessment_Topic_ToAssessment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAssessment_Topic] DROP CONSTRAINT [FK_AspNetAssessment_Topic_ToAssessment];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAssessment_Topic_ToTopic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAssessment_Topic] DROP CONSTRAINT [FK_AspNetAssessment_Topic_ToTopic];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAssessment_ToSubjectCatalog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAssessment] DROP CONSTRAINT [FK_AspNetAssessment_ToSubjectCatalog];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetAttendance_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetAttendance] DROP CONSTRAINT [FK_AspNetAttendance_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetChapter_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetChapter] DROP CONSTRAINT [FK_AspNetChapter_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetClass_FeeType_ToClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetClass_FeeType] DROP CONSTRAINT [FK_AspNetClass_FeeType_ToClass];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetClass_FeeType_ToFeeType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetClass_FeeType] DROP CONSTRAINT [FK_AspNetClass_FeeType_ToFeeType];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetClass_ToAspNetClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSubjects] DROP CONSTRAINT [FK_AspNetClass_ToAspNetClass];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetClass_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetClass] DROP CONSTRAINT [FK_AspNetClass_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetClass_ToAspNetUsersTeacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSubjects] DROP CONSTRAINT [FK_AspNetClass_ToAspNetUsersTeacher];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetEmployee_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetEmployee] DROP CONSTRAINT [FK_AspNetEmployee_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetEmployee_Attendance_TO_AspNetEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetEmployee_Attendance] DROP CONSTRAINT [FK_AspNetEmployee_Attendance_TO_AspNetEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetEmployee_Attendance_To_AspNetEmployeeAttendance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetEmployee_Attendance] DROP CONSTRAINT [FK_AspNetEmployee_Attendance_To_AspNetEmployeeAttendance];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetEmployee_VirtualRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetEmployee] DROP CONSTRAINT [FK_AspNetEmployee_VirtualRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetFeeChallan_ToClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetFeeChallan] DROP CONSTRAINT [FK_AspNetFeeChallan_ToClass];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetFeeChallan_ToDurationType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetFeeChallan] DROP CONSTRAINT [FK_AspNetFeeChallan_ToDurationType];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetFeedBackForm_ToRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetFeedBackForm] DROP CONSTRAINT [FK_AspNetFeedBackForm_ToRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetFeeType_ToLedger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetFeeType] DROP CONSTRAINT [FK_AspNetFeeType_ToLedger];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetFinanceMonth_AspNetFinancePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetFinanceMonth] DROP CONSTRAINT [FK_AspNetFinanceMonth_AspNetFinancePeriod];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetHomework_AspNetClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetHomework] DROP CONSTRAINT [FK_AspNetHomework_AspNetClass];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetLessonPlan_Topic_ToLessonPlan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLessonPlan_Topic] DROP CONSTRAINT [FK_AspNetLessonPlan_Topic_ToLessonPlan];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetLessonPlan_Topic_ToTopic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLessonPlan_Topic] DROP CONSTRAINT [FK_AspNetLessonPlan_Topic_ToTopic];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetLessonPlan_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLessonPlan] DROP CONSTRAINT [FK_AspNetLessonPlan_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetLog_ToUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLog] DROP CONSTRAINT [FK_AspNetLog_ToUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetNotification_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetNotification] DROP CONSTRAINT [FK_AspNetNotification_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetNotification_User_ToNotification]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetNotification_User] DROP CONSTRAINT [FK_AspNetNotification_User_ToNotification];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetNotification_User_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetNotification_User] DROP CONSTRAINT [FK_AspNetNotification_User_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetParent_Child_ChildToUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetParent_Child] DROP CONSTRAINT [FK_AspNetParent_Child_ChildToUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetParent_Child_ParentToUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetParent_Child] DROP CONSTRAINT [FK_AspNetParent_Child_ParentToUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetParent_Notification_ParentToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetParent_Notification] DROP CONSTRAINT [FK_AspNetParent_Notification_ParentToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetParent_Notification_SenderToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetParent_Notification] DROP CONSTRAINT [FK_AspNetParent_Notification_SenderToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetParent_ToParent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetParent] DROP CONSTRAINT [FK_AspNetParent_ToParent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetProject_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetProject] DROP CONSTRAINT [FK_AspNetProject_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTM_ParentFeedback_ToForm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTM_ParentFeedback] DROP CONSTRAINT [FK_AspNetPTM_ParentFeedback_ToForm];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTM_ParentFeedback_ToPTM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTM_ParentFeedback] DROP CONSTRAINT [FK_AspNetPTM_ParentFeedback_ToPTM];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTM_TeacherFeedback_ToForm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTM_TeacherFeedback] DROP CONSTRAINT [FK_AspNetPTM_TeacherFeedback_ToForm];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTM_TeacherFeedback_ToPTM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTM_TeacherFeedback] DROP CONSTRAINT [FK_AspNetPTM_TeacherFeedback_ToPTM];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTMAttendance_ToMeeting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTMAttendance] DROP CONSTRAINT [FK_AspNetPTMAttendance_ToMeeting];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTMAttendance_ToParent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTMAttendance] DROP CONSTRAINT [FK_AspNetPTMAttendance_ToParent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetPTMAttendance_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPTMAttendance] DROP CONSTRAINT [FK_AspNetPTMAttendance_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSalary_VirtualID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSalary] DROP CONSTRAINT [FK_AspNetSalary_VirtualID];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSalaryDetail_AspNetEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSalaryDetail] DROP CONSTRAINT [FK_AspNetSalaryDetail_AspNetEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSalaryDetail_SalaryTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSalaryDetail] DROP CONSTRAINT [FK_AspNetSalaryDetail_SalaryTable];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Assessment_ToAssessment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Assessment_ToAssessment];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Assessment_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Assessment_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Attendance_ToAttendance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Attendance] DROP CONSTRAINT [FK_AspNetStudent_Attendance_ToAttendance];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Attendance_ToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Attendance] DROP CONSTRAINT [FK_AspNetStudent_Attendance_ToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Fine_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Fine] DROP CONSTRAINT [FK_AspNetStudent_Fine_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_HomeWork_AspNetHomework]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_HomeWork] DROP CONSTRAINT [FK_AspNetStudent_HomeWork_AspNetHomework];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_HomeWork_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_HomeWork] DROP CONSTRAINT [FK_AspNetStudent_HomeWork_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Notification_SenderToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Notification] DROP CONSTRAINT [FK_AspNetStudent_Notification_SenderToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Notification_StudentToUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Notification] DROP CONSTRAINT [FK_AspNetStudent_Notification_StudentToUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Payment_ToFeeChallan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Payment] DROP CONSTRAINT [FK_AspNetStudent_Payment_ToFeeChallan];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Payment_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Payment] DROP CONSTRAINT [FK_AspNetStudent_Payment_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_PaymentDetail_ToLedger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_PaymentDetail] DROP CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToLedger];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_PaymentDetail_ToStudentPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_PaymentDetail] DROP CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToStudentPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_PaymentDetail_ToStudentPaymentPrevious]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_PaymentDetail] DROP CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToStudentPaymentPrevious];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Project_ToProject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Project] DROP CONSTRAINT [FK_AspNetStudent_Project_ToProject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Project_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Project] DROP CONSTRAINT [FK_AspNetStudent_Project_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Term_Assessment_ToSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Term_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToSession];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Term_Assessment_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Term_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Term_Assessment_ToTerm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Term_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToTerm];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Term_Assessment_ToUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Term_Assessment] DROP CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudent_Term_Assessments_Answers_ToStudent_Term_Assessment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Term_Assessments_Answers] DROP CONSTRAINT [FK_AspNetStudent_Term_Assessments_Answers_ToStudent_Term_Assessment];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudentPerformanceReport_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudentPerformanceReport] DROP CONSTRAINT [FK_AspNetStudentPerformanceReport_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetStudentPerformanceReport_ToUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudentPerformanceReport] DROP CONSTRAINT [FK_AspNetStudentPerformanceReport_ToUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSubject_Catalog_ToCatalog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSubject_Catalog] DROP CONSTRAINT [FK_AspNetSubject_Catalog_ToCatalog];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSubject_Catalog_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSubject_Catalog] DROP CONSTRAINT [FK_AspNetSubject_Catalog_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetSubject_Homework_AspNetSubjects]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetSubject_Homework] DROP CONSTRAINT [FK_AspNetSubject_Homework_AspNetSubjects];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetTerm_ToSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetTerm] DROP CONSTRAINT [FK_AspNetTerm_ToSession];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetTopic_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetTopic] DROP CONSTRAINT [FK_AspNetTopic_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetVirtualRole_AspNetVirtualRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetVirtualRole] DROP CONSTRAINT [FK_AspNetVirtualRole_AspNetVirtualRole];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeePenalty_Employee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeePenalty] DROP CONSTRAINT [FK_EmployeePenalty_Employee];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeePenalty_Penalty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeePenalty] DROP CONSTRAINT [FK_EmployeePenalty_Penalty];
GO
IF OBJECT_ID(N'[dbo].[FK_FeeMonth]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Month_Multiplier] DROP CONSTRAINT [FK_FeeMonth];
GO
IF OBJECT_ID(N'[HangFire].[FK_HangFire_JobParameter_Job]', 'F') IS NOT NULL
    ALTER TABLE [HangFire].[JobParameter] DROP CONSTRAINT [FK_HangFire_JobParameter_Job];
GO
IF OBJECT_ID(N'[HangFire].[FK_HangFire_State_Job]', 'F') IS NOT NULL
    ALTER TABLE [HangFire].[State] DROP CONSTRAINT [FK_HangFire_State_Job];
GO
IF OBJECT_ID(N'[dbo].[FK_Ledger_LedgerGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ledger] DROP CONSTRAINT [FK_Ledger_LedgerGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_Ledger_LedgerHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ledger] DROP CONSTRAINT [FK_Ledger_LedgerHead];
GO
IF OBJECT_ID(N'[dbo].[FK_Ledger_Session]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ledger] DROP CONSTRAINT [FK_Ledger_Session];
GO
IF OBJECT_ID(N'[dbo].[FK_LedgerGroup_LedgerHead]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LedgerGroup] DROP CONSTRAINT [FK_LedgerGroup_LedgerHead];
GO
IF OBJECT_ID(N'[dbo].[FK_LessonPlanBreakdown_ToLessonPlan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLessonPlanBreakdown] DROP CONSTRAINT [FK_LessonPlanBreakdown_ToLessonPlan];
GO
IF OBJECT_ID(N'[dbo].[FK_LessonPlanBreakdown_ToLessonPlanBreakDownHeading]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetLessonPlanBreakdown] DROP CONSTRAINT [FK_LessonPlanBreakdown_ToLessonPlanBreakDownHeading];
GO
IF OBJECT_ID(N'[dbo].[FK_MessageReceiver_Messages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetMessage_Receiver] DROP CONSTRAINT [FK_MessageReceiver_Messages];
GO
IF OBJECT_ID(N'[dbo].[FK_MessageReceiver_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetMessage_Receiver] DROP CONSTRAINT [FK_MessageReceiver_User];
GO
IF OBJECT_ID(N'[dbo].[FK_MessagesSender_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetMessages] DROP CONSTRAINT [FK_MessagesSender_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_NonRecurring_Description]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NonRecurringFeeMultiplier] DROP CONSTRAINT [FK_NonRecurring_Description];
GO
IF OBJECT_ID(N'[dbo].[FK_NonRecurring_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NonRecurringFeeMultiplier] DROP CONSTRAINT [FK_NonRecurring_student];
GO
IF OBJECT_ID(N'[dbo].[FK_Notifications_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetPushNotifications] DROP CONSTRAINT [FK_Notifications_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_RecurringFeeClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentRecurringFee] DROP CONSTRAINT [FK_RecurringFeeClass];
GO
IF OBJECT_ID(N'[dbo].[FK_RecurringFeeClass_Session]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentRecurringFee] DROP CONSTRAINT [FK_RecurringFeeClass_Session];
GO
IF OBJECT_ID(N'[dbo].[FK_SalaryIncrement_SalaryRecord]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee_SalaryIncrement] DROP CONSTRAINT [FK_SalaryIncrement_SalaryRecord];
GO
IF OBJECT_ID(N'[dbo].[FK_SalaryRecord_EmployeeId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employee_SalaryRecord] DROP CONSTRAINT [FK_SalaryRecord_EmployeeId];
GO
IF OBJECT_ID(N'[dbo].[FK_SalaryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalary] DROP CONSTRAINT [FK_SalaryUser];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_Announcement_ToAnnouncement]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Announcement] DROP CONSTRAINT [FK_Student_Announcement_ToAnnouncement];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_Announcement_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Announcement] DROP CONSTRAINT [FK_Student_Announcement_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_Subject_ToStudent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Subject] DROP CONSTRAINT [FK_Student_Subject_ToStudent];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_Subject_ToSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent_Subject] DROP CONSTRAINT [FK_Student_Subject_ToSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_ToAspNetClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent] DROP CONSTRAINT [FK_Student_ToAspNetClass];
GO
IF OBJECT_ID(N'[dbo].[FK_Student_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetStudent] DROP CONSTRAINT [FK_Student_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentChallan_Student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentChallanForm] DROP CONSTRAINT [FK_StudentChallan_Student];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentChallan_StudentFeeMonth]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentChallanForm] DROP CONSTRAINT [FK_StudentChallan_StudentFeeMonth];
GO
IF OBJECT_ID(N'[dbo].[FK_studentdiscount_Feedisocunt]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentDiscount] DROP CONSTRAINT [FK_studentdiscount_Feedisocunt];
GO
IF OBJECT_ID(N'[dbo].[FK_studentdiscount_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentDiscount] DROP CONSTRAINT [FK_studentdiscount_student];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentFeeMonth_AspNetSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentFeeMonth] DROP CONSTRAINT [FK_StudentFeeMonth_AspNetSession];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentFeeMonth_student]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentFeeMonth] DROP CONSTRAINT [FK_StudentFeeMonth_student];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentPenalty_Penalty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentPenalty] DROP CONSTRAINT [FK_StudentPenalty_Penalty];
GO
IF OBJECT_ID(N'[dbo].[FK_StudentPenalty_Stuedent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StudentPenalty] DROP CONSTRAINT [FK_StudentPenalty_Stuedent];
GO
IF OBJECT_ID(N'[dbo].[FK_Teacher_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetTeacher] DROP CONSTRAINT [FK_Teacher_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_ToDoList_ToAspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ToDoList] DROP CONSTRAINT [FK_ToDoList_ToAspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_VoucherRecord_LedgerId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoucherRecord] DROP CONSTRAINT [FK_VoucherRecord_LedgerId];
GO
IF OBJECT_ID(N'[dbo].[FK_VoucherRecord_Voucher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VoucherRecord] DROP CONSTRAINT [FK_VoucherRecord_Voucher];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetAbsent_Student]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAbsent_Student];
GO
IF OBJECT_ID(N'[dbo].[AspNetAnnouncement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAnnouncement];
GO
IF OBJECT_ID(N'[dbo].[AspNetAnnouncement_Subject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAnnouncement_Subject];
GO
IF OBJECT_ID(N'[dbo].[AspNetAssessment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAssessment];
GO
IF OBJECT_ID(N'[dbo].[AspNetAssessment_Question]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAssessment_Question];
GO
IF OBJECT_ID(N'[dbo].[AspNetAssessment_Questions_Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAssessment_Questions_Category];
GO
IF OBJECT_ID(N'[dbo].[AspNetAssessment_Topic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAssessment_Topic];
GO
IF OBJECT_ID(N'[dbo].[AspNetAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetAttendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetCatalog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetCatalog];
GO
IF OBJECT_ID(N'[dbo].[AspNetChapter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetChapter];
GO
IF OBJECT_ID(N'[dbo].[AspNetClass]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetClass];
GO
IF OBJECT_ID(N'[dbo].[AspNetClass_FeeType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetClass_FeeType];
GO
IF OBJECT_ID(N'[dbo].[AspNetCurriculum]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetCurriculum];
GO
IF OBJECT_ID(N'[dbo].[AspNetDurationType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetDurationType];
GO
IF OBJECT_ID(N'[dbo].[AspNetEmp_Auto_Absent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetEmp_Auto_Absent];
GO
IF OBJECT_ID(N'[dbo].[AspNetEmp_Auto_Attendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetEmp_Auto_Attendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetEmployee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetEmployee];
GO
IF OBJECT_ID(N'[dbo].[AspNetEmployee_Attendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetEmployee_Attendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetEmployeeAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetEmployeeAttendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetFeeChallan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFeeChallan];
GO
IF OBJECT_ID(N'[dbo].[AspNetFeedBackForm]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFeedBackForm];
GO
IF OBJECT_ID(N'[dbo].[AspNetFeeType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFeeType];
GO
IF OBJECT_ID(N'[dbo].[AspNetFinanceLedgers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFinanceLedgers];
GO
IF OBJECT_ID(N'[dbo].[AspNetFinanceLedgerType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFinanceLedgerType];
GO
IF OBJECT_ID(N'[dbo].[AspNetFinanceMonth]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFinanceMonth];
GO
IF OBJECT_ID(N'[dbo].[AspNetFinancePeriod]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetFinancePeriod];
GO
IF OBJECT_ID(N'[dbo].[AspNetHoliday_Calendar_Notification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetHoliday_Calendar_Notification];
GO
IF OBJECT_ID(N'[dbo].[AspNetHomework]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetHomework];
GO
IF OBJECT_ID(N'[dbo].[AspNetLessonPlan]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLessonPlan];
GO
IF OBJECT_ID(N'[dbo].[AspNetLessonPlan_Topic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLessonPlan_Topic];
GO
IF OBJECT_ID(N'[dbo].[AspNetLessonPlanBreakdown]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLessonPlanBreakdown];
GO
IF OBJECT_ID(N'[dbo].[AspNetLessonPlanBreakdownHeading]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLessonPlanBreakdownHeading];
GO
IF OBJECT_ID(N'[dbo].[AspNetLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLog];
GO
IF OBJECT_ID(N'[dbo].[AspNetLoginTime]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetLoginTime];
GO
IF OBJECT_ID(N'[dbo].[AspNetMessage_Receiver]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetMessage_Receiver];
GO
IF OBJECT_ID(N'[dbo].[AspNetMessages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetMessages];
GO
IF OBJECT_ID(N'[dbo].[AspNetNotification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetNotification];
GO
IF OBJECT_ID(N'[dbo].[AspNetNotification_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetNotification_User];
GO
IF OBJECT_ID(N'[dbo].[AspNetParent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetParent];
GO
IF OBJECT_ID(N'[dbo].[AspNetParent_Child]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetParent_Child];
GO
IF OBJECT_ID(N'[dbo].[AspNetParent_Notification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetParent_Notification];
GO
IF OBJECT_ID(N'[dbo].[AspNetParentTeacherMeeting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetParentTeacherMeeting];
GO
IF OBJECT_ID(N'[dbo].[AspNetProject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetProject];
GO
IF OBJECT_ID(N'[dbo].[AspNetPTM_ParentFeedback]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetPTM_ParentFeedback];
GO
IF OBJECT_ID(N'[dbo].[AspNetPTM_TeacherFeedback]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetPTM_TeacherFeedback];
GO
IF OBJECT_ID(N'[dbo].[AspNetPTMAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetPTMAttendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetPTMFormRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetPTMFormRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetPushNotifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetPushNotifications];
GO
IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetSalary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSalary];
GO
IF OBJECT_ID(N'[dbo].[AspNetSalaryDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSalaryDetail];
GO
IF OBJECT_ID(N'[dbo].[AspNetSession]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSession];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Announcement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Announcement];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Assessment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Assessment];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Attendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Attendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_AutoAttendance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_AutoAttendance];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Fine]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Fine];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_HomeWork]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_HomeWork];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Notification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Notification];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Payment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Payment];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_PaymentDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_PaymentDetail];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Project]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Project];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Subject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Subject];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Term_Assessment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Term_Assessment];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudent_Term_Assessments_Answers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudent_Term_Assessments_Answers];
GO
IF OBJECT_ID(N'[dbo].[AspNetStudentPerformanceReport]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetStudentPerformanceReport];
GO
IF OBJECT_ID(N'[dbo].[AspNetSubject_Catalog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSubject_Catalog];
GO
IF OBJECT_ID(N'[dbo].[AspNetSubject_Homework]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSubject_Homework];
GO
IF OBJECT_ID(N'[dbo].[AspNetSubjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetSubjects];
GO
IF OBJECT_ID(N'[dbo].[AspNetTeacher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetTeacher];
GO
IF OBJECT_ID(N'[dbo].[AspNetTerm]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetTerm];
GO
IF OBJECT_ID(N'[dbo].[AspNetTime_Setting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetTime_Setting];
GO
IF OBJECT_ID(N'[dbo].[AspNetTopic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetTopic];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[AspNetVirtualRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetVirtualRole];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[Employee_SalaryIncrement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee_SalaryIncrement];
GO
IF OBJECT_ID(N'[dbo].[Employee_SalaryRecord]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee_SalaryRecord];
GO
IF OBJECT_ID(N'[dbo].[EmployeeAdvanceSalary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeAdvanceSalary];
GO
IF OBJECT_ID(N'[dbo].[EmployeePenalty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeePenalty];
GO
IF OBJECT_ID(N'[dbo].[EmployeePenaltyType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeePenaltyType];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalary];
GO
IF OBJECT_ID(N'[dbo].[Event]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Event];
GO
IF OBJECT_ID(N'[dbo].[FeeDateSetting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FeeDateSetting];
GO
IF OBJECT_ID(N'[dbo].[FeeDiscount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FeeDiscount];
GO
IF OBJECT_ID(N'[dbo].[Ledger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ledger];
GO
IF OBJECT_ID(N'[dbo].[LedgerGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LedgerGroup];
GO
IF OBJECT_ID(N'[dbo].[LedgerHead]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LedgerHead];
GO
IF OBJECT_ID(N'[dbo].[Month_Multiplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Month_Multiplier];
GO
IF OBJECT_ID(N'[dbo].[NonRecurringCharges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NonRecurringCharges];
GO
IF OBJECT_ID(N'[dbo].[NonRecurringFeeMultiplier]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NonRecurringFeeMultiplier];
GO
IF OBJECT_ID(N'[dbo].[PenaltyFee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PenaltyFee];
GO
IF OBJECT_ID(N'[dbo].[ruffdata]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ruffdata];
GO
IF OBJECT_ID(N'[dbo].[Student_ChallanForm]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Student_ChallanForm];
GO
IF OBJECT_ID(N'[dbo].[StudentChallanForm]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentChallanForm];
GO
IF OBJECT_ID(N'[dbo].[StudentDiscount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentDiscount];
GO
IF OBJECT_ID(N'[dbo].[StudentFeeMonth]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentFeeMonth];
GO
IF OBJECT_ID(N'[dbo].[StudentPenalty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentPenalty];
GO
IF OBJECT_ID(N'[dbo].[StudentPenaltyType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentPenaltyType];
GO
IF OBJECT_ID(N'[dbo].[StudentRecurringFee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudentRecurringFee];
GO
IF OBJECT_ID(N'[dbo].[ToDoList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ToDoList];
GO
IF OBJECT_ID(N'[dbo].[Voucher]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Voucher];
GO
IF OBJECT_ID(N'[dbo].[VoucherRecord]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VoucherRecord];
GO
IF OBJECT_ID(N'[HangFire].[AggregatedCounter]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[AggregatedCounter];
GO
IF OBJECT_ID(N'[HangFire].[Counter]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Counter];
GO
IF OBJECT_ID(N'[HangFire].[Hash]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Hash];
GO
IF OBJECT_ID(N'[HangFire].[Job]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Job];
GO
IF OBJECT_ID(N'[HangFire].[JobParameter]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[JobParameter];
GO
IF OBJECT_ID(N'[HangFire].[JobQueue]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[JobQueue];
GO
IF OBJECT_ID(N'[HangFire].[List]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[List];
GO
IF OBJECT_ID(N'[HangFire].[Schema]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Schema];
GO
IF OBJECT_ID(N'[HangFire].[Server]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Server];
GO
IF OBJECT_ID(N'[HangFire].[Set]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[Set];
GO
IF OBJECT_ID(N'[HangFire].[State]', 'U') IS NOT NULL
    DROP TABLE [HangFire].[State];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetAbsent_Student'
CREATE TABLE [dbo].[AspNetAbsent_Student] (
    [AbsentId] int IDENTITY(1,1) NOT NULL,
    [Roll_Number] varchar(25)  NULL,
    [Date] datetime  NULL,
    [Class] varchar(50)  NULL
);
GO

-- Creating table 'AspNetAnnouncements'
CREATE TABLE [dbo].[AspNetAnnouncements] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(max)  NULL,
    [Description] varchar(max)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'AspNetAnnouncement_Subject'
CREATE TABLE [dbo].[AspNetAnnouncement_Subject] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectID] int  NULL,
    [AnnouncementID] int  NULL
);
GO

-- Creating table 'AspNetAssessments'
CREATE TABLE [dbo].[AspNetAssessments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject_CatalogID] int  NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [PublishDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [TotalMarks] int  NULL,
    [Weightage] int  NULL,
    [FileName] nvarchar(max)  NULL,
    [AcceptSubmission] bit  NULL
);
GO

-- Creating table 'AspNetAssessment_Question'
CREATE TABLE [dbo].[AspNetAssessment_Question] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Question] nvarchar(max)  NULL,
    [SubjectID] int  NULL,
    [QuestionCategory] int  NULL
);
GO

-- Creating table 'AspNetAssessment_Questions_Category'
CREATE TABLE [dbo].[AspNetAssessment_Questions_Category] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetAssessment_Topic'
CREATE TABLE [dbo].[AspNetAssessment_Topic] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssessmentID] int  NULL,
    [TopicID] int  NULL
);
GO

-- Creating table 'AspNetAttendances'
CREATE TABLE [dbo].[AspNetAttendances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectID] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'AspNetCatalogs'
CREATE TABLE [dbo].[AspNetCatalogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CatalogName] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetChapters'
CREATE TABLE [dbo].[AspNetChapters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ChapterNo] float  NULL,
    [ChapterName] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [SubjectID] int  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'AspNetClasses'
CREATE TABLE [dbo].[AspNetClasses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassName] nvarchar(max)  NULL,
    [Class] nvarchar(max)  NULL,
    [Section] nvarchar(10)  NULL,
    [TeacherID] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetClass_FeeType'
CREATE TABLE [dbo].[AspNetClass_FeeType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassID] int  NULL,
    [LedgerID] int  NULL,
    [Amount] int  NULL,
    [Type] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetCurriculums'
CREATE TABLE [dbo].[AspNetCurriculums] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CurriculumName] varchar(max)  NULL
);
GO

-- Creating table 'AspNetDurationTypes'
CREATE TABLE [dbo].[AspNetDurationTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(max)  NULL,
    [MonthsDuration] int  NULL
);
GO

-- Creating table 'AspNetEmp_Auto_Absent'
CREATE TABLE [dbo].[AspNetEmp_Auto_Absent] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NULL,
    [PhoneNumber] varchar(50)  NULL,
    [Name] varchar(50)  NULL
);
GO

-- Creating table 'AspNetEmp_Auto_Attendance'
CREATE TABLE [dbo].[AspNetEmp_Auto_Attendance] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NULL,
    [TimeIn] time  NULL,
    [PhoneNumber] varchar(50)  NULL,
    [TimeOut] time  NULL,
    [Name] varchar(50)  NULL
);
GO

-- Creating table 'AspNetEmployees'
CREATE TABLE [dbo].[AspNetEmployees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PositionAppliedFor] nvarchar(max)  NULL,
    [DateAvailable] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [BirthDate] nvarchar(max)  NULL,
    [Nationality] nvarchar(max)  NULL,
    [Religion] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [MailingAddress] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [CellNo] nvarchar(max)  NULL,
    [Landline] nvarchar(max)  NULL,
    [SpouseName] nvarchar(max)  NULL,
    [SpouseHighestDegree] nvarchar(max)  NULL,
    [SpouseOccupation] nvarchar(max)  NULL,
    [SpouseBusinessAddress] nvarchar(max)  NULL,
    [UserId] nvarchar(128)  NULL,
    [GrossSalary] int  NULL,
    [BasicSalary] int  NULL,
    [MedicalAllowance] int  NULL,
    [Accomodation] int  NULL,
    [ProvidedFund] int  NULL,
    [Tax] int  NULL,
    [EOP] int  NULL,
    [Salary] int  NULL,
    [UserName] nvarchar(50)  NULL,
    [VirtualRoleId] int  NULL,
    [JoiningDate] nvarchar(max)  NULL,
    [Illness] nvarchar(max)  NULL,
    [Status] nvarchar(20)  NULL
);
GO

-- Creating table 'AspNetEmployee_Attendance'
CREATE TABLE [dbo].[AspNetEmployee_Attendance] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AttendanceID] int  NULL,
    [EmployeeID] int  NULL,
    [Status] nvarchar(50)  NULL,
    [Reason] nvarchar(50)  NULL,
    [Month] nvarchar(50)  NULL,
    [Time] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetEmployeeAttendances'
CREATE TABLE [dbo].[AspNetEmployeeAttendances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'AspNetFeeChallans'
CREATE TABLE [dbo].[AspNetFeeChallans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassID] int  NULL,
    [DueDate] datetime  NULL,
    [DurationTypeID] int  NULL,
    [TotalAmount] int  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Penalty] int  NULL,
    [ValidDate] datetime  NULL,
    [Title] nvarchar(50)  NULL,
    [Created] datetime  NULL
);
GO

-- Creating table 'AspNetFeedBackForms'
CREATE TABLE [dbo].[AspNetFeedBackForms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Question] nvarchar(max)  NULL,
    [FormFor] int  NULL
);
GO

-- Creating table 'AspNetFeeTypes'
CREATE TABLE [dbo].[AspNetFeeTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LedgerID] int  NULL
);
GO

-- Creating table 'AspNetFinanceLedgers'
CREATE TABLE [dbo].[AspNetFinanceLedgers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NULL,
    [Name] nvarchar(50)  NULL,
    [Type] nvarchar(50)  NULL,
    [Balance] nvarchar(50)  NULL,
    [IsActive] bit  NULL,
    [IsGroup] bit  NULL,
    [TakeAble] bit  NULL,
    [Head] int  NOT NULL,
    [StartingBalance] float  NULL,
    [ShowIndividual] nvarchar(10)  NULL
);
GO

-- Creating table 'AspNetFinanceLedgerTypes'
CREATE TABLE [dbo].[AspNetFinanceLedgerTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NULL
);
GO

-- Creating table 'AspNetFinanceMonths'
CREATE TABLE [dbo].[AspNetFinanceMonths] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Month] varchar(50)  NULL,
    [StartData] datetime  NULL,
    [EndDate] datetime  NULL,
    [Name] varchar(50)  NULL,
    [PeriodId] int  NULL
);
GO

-- Creating table 'AspNetFinancePeriods'
CREATE TABLE [dbo].[AspNetFinancePeriods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Year] nchar(60)  NULL
);
GO

-- Creating table 'AspNetHoliday_Calendar_Notification'
CREATE TABLE [dbo].[AspNetHoliday_Calendar_Notification] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] varchar(250)  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL
);
GO

-- Creating table 'AspNetHomeworks'
CREATE TABLE [dbo].[AspNetHomeworks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassId] int  NULL,
    [Date] datetime  NULL,
    [PrincipalApproved_status] varchar(max)  NULL
);
GO

-- Creating table 'AspNetLessonPlans'
CREATE TABLE [dbo].[AspNetLessonPlans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LessonPlanNo] int  NULL,
    [Date] datetime  NULL,
    [Duration] int  NULL,
    [SubjectID] int  NULL
);
GO

-- Creating table 'AspNetLessonPlan_Topic'
CREATE TABLE [dbo].[AspNetLessonPlan_Topic] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LessonPlanID] int  NULL,
    [TopicID] int  NULL
);
GO

-- Creating table 'AspNetLessonPlanBreakdowns'
CREATE TABLE [dbo].[AspNetLessonPlanBreakdowns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LessonPlanID] int  NULL,
    [BreakDownHeadingID] int  NULL,
    [Description] nvarchar(max)  NULL,
    [Minutes] int  NULL,
    [Resources] nvarchar(max)  NULL,
    [Priority] int  NULL
);
GO

-- Creating table 'AspNetLessonPlanBreakdownHeadings'
CREATE TABLE [dbo].[AspNetLessonPlanBreakdownHeadings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BreakDownHeadingName] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetLogs'
CREATE TABLE [dbo].[AspNetLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Operation] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [UserID] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetLoginTimes'
CREATE TABLE [dbo].[AspNetLoginTimes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [Duration] datetime  NULL,
    [UserName] nvarchar(50)  NULL,
    [UserId] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetMessage_Receiver'
CREATE TABLE [dbo].[AspNetMessage_Receiver] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MessageID] int  NULL,
    [ReceiverID] nvarchar(128)  NULL,
    [Seen] nvarchar(50)  NOT NULL,
    [SeenTime] datetime  NOT NULL
);
GO

-- Creating table 'AspNetMessages'
CREATE TABLE [dbo].[AspNetMessages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(500)  NULL,
    [Message] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [SenderID] nvarchar(128)  NULL,
    [IsEmail] bit  NULL,
    [IsText] bit  NULL
);
GO

-- Creating table 'AspNetNotifications'
CREATE TABLE [dbo].[AspNetNotifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [SenderID] nvarchar(128)  NULL,
    [Url] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetNotification_User'
CREATE TABLE [dbo].[AspNetNotification_User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(128)  NULL,
    [NotificationID] int  NULL,
    [Seen] bit  NULL
);
GO

-- Creating table 'AspNetParents'
CREATE TABLE [dbo].[AspNetParents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(128)  NULL,
    [FatherName] nvarchar(max)  NULL,
    [FatherCellNo] nvarchar(max)  NULL,
    [FatherEmail] nvarchar(max)  NULL,
    [FatherOccupation] nvarchar(max)  NULL,
    [FatherEmployer] nvarchar(max)  NULL,
    [MotherName] nvarchar(max)  NULL,
    [MotherCellNo] nvarchar(max)  NULL,
    [MotherEmail] nvarchar(max)  NULL,
    [MotherOccupation] nvarchar(max)  NULL,
    [MotherEmployer] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetParent_Child'
CREATE TABLE [dbo].[AspNetParent_Child] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentID] nvarchar(128)  NULL,
    [ChildID] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetParent_Notification'
CREATE TABLE [dbo].[AspNetParent_Notification] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentID] nvarchar(128)  NULL,
    [SenderID] nvarchar(128)  NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL,
    [Time] datetime  NULL
);
GO

-- Creating table 'AspNetParentTeacherMeetings'
CREATE TABLE [dbo].[AspNetParentTeacherMeetings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [Time] nvarchar(50)  NULL,
    [Status] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetProjects'
CREATE TABLE [dbo].[AspNetProjects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [PublishDate] datetime  NULL,
    [DueDate] datetime  NULL,
    [FileName] nvarchar(max)  NULL,
    [AcceptSubmission] bit  NOT NULL,
    [SubjectID] int  NULL
);
GO

-- Creating table 'AspNetPTM_ParentFeedback'
CREATE TABLE [dbo].[AspNetPTM_ParentFeedback] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PTMID] int  NULL,
    [HeadingID] int  NULL,
    [FeedBack] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetPTM_TeacherFeedback'
CREATE TABLE [dbo].[AspNetPTM_TeacherFeedback] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PTMID] int  NULL,
    [HeadingID] int  NULL,
    [FeedBack] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetPTMAttendances'
CREATE TABLE [dbo].[AspNetPTMAttendances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MeetingID] int  NULL,
    [ParentID] nvarchar(128)  NULL,
    [SubjectID] int  NULL,
    [Status] nvarchar(50)  NULL,
    [Rating] int  NULL
);
GO

-- Creating table 'AspNetPTMFormRoles'
CREATE TABLE [dbo].[AspNetPTMFormRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(128)  NULL
);
GO

-- Creating table 'AspNetPushNotifications'
CREATE TABLE [dbo].[AspNetPushNotifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NULL,
    [UserID] nvarchar(128)  NULL,
    [Time] datetime  NULL,
    [IsOpenStudent] bit  NULL,
    [IsOpenParent] bit  NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [VirtualId] int  NULL
);
GO

-- Creating table 'AspNetSalaries'
CREATE TABLE [dbo].[AspNetSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Month] nvarchar(50)  NULL,
    [Title] nvarchar(50)  NULL,
    [VirtualRoleID] int  NULL,
    [Year] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetSalaryDetails'
CREATE TABLE [dbo].[AspNetSalaryDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GrossSalary] int  NULL,
    [MedicalAllowance] int  NULL,
    [AccomodationAllowance] int  NULL,
    [Tax] int  NULL,
    [BasicSalary] int  NULL,
    [FineCut] int  NULL,
    [AfterCutSalary] int  NULL,
    [AdvancePf] int  NULL,
    [EmployeePF] int  NULL,
    [AdvanceSalary] int  NULL,
    [EmployeeEOP] int  NULL,
    [Total] int  NULL,
    [SchoolEOP] int  NULL,
    [BTaxSalary] int  NULL,
    [ATaxSalary] int  NULL,
    [Bonus] int  NULL,
    [SalaryHold] int  NULL,
    [TotalSalary] int  NULL,
    [Status] nvarchar(50)  NULL,
    [EmployeeId] int  NOT NULL,
    [SalaryId] int  NOT NULL
);
GO

-- Creating table 'AspNetSessions'
CREATE TABLE [dbo].[AspNetSessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SessionName] nvarchar(max)  NULL,
    [SessionStartDate] datetime  NULL,
    [SessionEndDate] datetime  NULL,
    [Status] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetStudents'
CREATE TABLE [dbo].[AspNetStudents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NOT NULL,
    [ClassID] int  NULL,
    [Level] nvarchar(max)  NULL,
    [SchoolName] nvarchar(max)  NULL,
    [BirthDate] nvarchar(max)  NULL,
    [Nationality] nvarchar(max)  NULL,
    [Religion] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetStudent_Announcement'
CREATE TABLE [dbo].[AspNetStudent_Announcement] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NOT NULL,
    [AnnouncementID] int  NULL,
    [Seen] bit  NULL
);
GO

-- Creating table 'AspNetStudent_Assessment'
CREATE TABLE [dbo].[AspNetStudent_Assessment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssessmentID] int  NULL,
    [StudentID] nvarchar(128)  NULL,
    [MarksGot] int  NULL,
    [SubmissionStatus] bit  NULL,
    [SubmittedFileName] nvarchar(max)  NULL,
    [SubmissionDate] datetime  NULL
);
GO

-- Creating table 'AspNetStudent_Attendance'
CREATE TABLE [dbo].[AspNetStudent_Attendance] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AttendanceID] int  NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [Status] nvarchar(50)  NULL,
    [Reason] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetStudent_AutoAttendance'
CREATE TABLE [dbo].[AspNetStudent_AutoAttendance] (
    [Attendance_id] int IDENTITY(1,1) NOT NULL,
    [Roll_Number] varchar(50)  NULL,
    [Date] datetime  NULL,
    [TimeIn] time  NOT NULL,
    [TimeOut] time  NULL,
    [Class] varchar(50)  NULL
);
GO

-- Creating table 'AspNetStudent_Fine'
CREATE TABLE [dbo].[AspNetStudent_Fine] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [FineDetail] nvarchar(max)  NULL,
    [Amount] int  NULL,
    [Status] nvarchar(50)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'AspNetStudent_HomeWork'
CREATE TABLE [dbo].[AspNetStudent_HomeWork] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [HomeworkID] int  NULL,
    [Reading] nvarchar(max)  NULL,
    [TeacherComments] nvarchar(max)  NULL,
    [ParentComments] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL
);
GO

-- Creating table 'AspNetStudent_Notification'
CREATE TABLE [dbo].[AspNetStudent_Notification] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [SenderID] nvarchar(128)  NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Attachment] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL,
    [Time] datetime  NULL
);
GO

-- Creating table 'AspNetStudent_Payment'
CREATE TABLE [dbo].[AspNetStudent_Payment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [FeeChallanID] int  NULL,
    [TotalAmount] int  NULL,
    [PaymentAmount] int  NULL,
    [PaymentDate] datetime  NULL,
    [Status] nvarchar(max)  NULL,
    [Remarks] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetStudent_PaymentDetail'
CREATE TABLE [dbo].[AspNetStudent_PaymentDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Student_PaymentID] int  NULL,
    [LedgerID] int  NULL,
    [Amount] int  NULL,
    [PreviousFeeID] int  NULL
);
GO

-- Creating table 'AspNetStudent_Project'
CREATE TABLE [dbo].[AspNetStudent_Project] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectID] int  NULL,
    [StudentID] nvarchar(128)  NULL,
    [SubmissionStatus] bit  NULL,
    [SubmittedFileName] nvarchar(max)  NULL,
    [SubmissionDate] datetime  NULL
);
GO

-- Creating table 'AspNetStudent_Subject'
CREATE TABLE [dbo].[AspNetStudent_Subject] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NOT NULL,
    [SubjectID] int  NOT NULL
);
GO

-- Creating table 'AspNetStudent_Term_Assessment'
CREATE TABLE [dbo].[AspNetStudent_Term_Assessment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [TermID] int  NULL,
    [SubjectID] int  NULL,
    [TeacherComments] nvarchar(max)  NULL,
    [PrincipalComments] nvarchar(max)  NULL,
    [ParentsComments] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL,
    [Date] datetime  NULL,
    [NotifyTeacher] bit  NULL,
    [SessionId] int  NULL
);
GO

-- Creating table 'AspNetStudent_Term_Assessments_Answers'
CREATE TABLE [dbo].[AspNetStudent_Term_Assessments_Answers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [STAID] int  NULL,
    [Answer] nvarchar(50)  NULL,
    [Catageory] nvarchar(50)  NULL,
    [FirstTermGrade] nvarchar(50)  NULL,
    [SecondTermGrade] nvarchar(50)  NULL,
    [ThirdTermGrade] nvarchar(50)  NULL,
    [Question] varchar(50)  NULL
);
GO

-- Creating table 'AspNetStudentPerformanceReports'
CREATE TABLE [dbo].[AspNetStudentPerformanceReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentID] nvarchar(128)  NULL,
    [SubjectID] int  NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [LearningConcept] nvarchar(50)  NULL,
    [TimeManagement] nvarchar(50)  NULL,
    [Homework] nvarchar(50)  NULL,
    [ReadingSkills] nvarchar(50)  NULL,
    [InstructionFollowing] nvarchar(50)  NULL,
    [ProjectLines] nvarchar(50)  NULL,
    [Handwriting] nvarchar(50)  NULL,
    [PerformanceSkills] nvarchar(50)  NULL,
    [Punctuality] nvarchar(50)  NULL,
    [Regularity] nvarchar(50)  NULL,
    [Assessment] nvarchar(50)  NULL,
    [Date] datetime  NULL
);
GO

-- Creating table 'AspNetSubject_Catalog'
CREATE TABLE [dbo].[AspNetSubject_Catalog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectID] int  NULL,
    [CatalogID] int  NULL,
    [Weightage] int  NULL
);
GO

-- Creating table 'AspNetSubject_Homework'
CREATE TABLE [dbo].[AspNetSubject_Homework] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectID] int  NULL,
    [HomeworkDetail] nvarchar(max)  NULL,
    [HomeworkID] int  NULL
);
GO

-- Creating table 'AspNetSubjects'
CREATE TABLE [dbo].[AspNetSubjects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubjectName] nvarchar(max)  NULL,
    [ClassID] int  NULL,
    [TeacherID] nvarchar(128)  NULL,
    [Status] varchar(20)  NULL
);
GO

-- Creating table 'AspNetTeachers'
CREATE TABLE [dbo].[AspNetTeachers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TeacherID] nvarchar(128)  NOT NULL,
    [JoiningDate] datetime  NULL,
    [DateAvailable] nvarchar(max)  NULL,
    [PositionAppliedFor] nvarchar(max)  NULL,
    [BirthDate] nvarchar(max)  NULL,
    [Nationality] nvarchar(max)  NULL,
    [Religion] nvarchar(max)  NULL,
    [Gender] nvarchar(max)  NULL,
    [MailingAddress] nvarchar(max)  NULL,
    [CellNo] nvarchar(max)  NULL,
    [Landline] nvarchar(max)  NULL,
    [SpouseName] nvarchar(max)  NULL,
    [SpouseHeighestDegree] nvarchar(max)  NULL,
    [SpouseOccupation] nvarchar(max)  NULL,
    [SpouseBusinessAddress] nvarchar(max)  NULL,
    [DegreeCertificate] nvarchar(max)  NULL,
    [MajorSubjects] nvarchar(max)  NULL,
    [Year] nvarchar(max)  NULL,
    [SchoolCollageUniversityCity] nvarchar(max)  NULL,
    [Illness] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetTerms'
CREATE TABLE [dbo].[AspNetTerms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TermName] nvarchar(max)  NULL,
    [SessionID] int  NULL,
    [TermStartDate] datetime  NULL,
    [TermEndDate] datetime  NULL,
    [Status] nvarchar(max)  NULL,
    [TermNo] int  NULL
);
GO

-- Creating table 'AspNetTime_Setting'
CREATE TABLE [dbo].[AspNetTime_Setting] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AbsentTime] time  NOT NULL,
    [LateTime] time  NULL
);
GO

-- Creating table 'AspNetTopics'
CREATE TABLE [dbo].[AspNetTopics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TopicNo] float  NULL,
    [TopicName] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [ChapterID] int  NULL,
    [Status] bit  NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Status] nvarchar(6)  NULL,
    [Image] nvarchar(max)  NULL,
    [LastPasswordChange] datetime  NULL
);
GO

-- Creating table 'AspNetVirtualRoles'
CREATE TABLE [dbo].[AspNetVirtualRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Employee_SalaryIncrement'
CREATE TABLE [dbo].[Employee_SalaryIncrement] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PreviousSalary] float  NULL,
    [AfterSalary] float  NULL,
    [TimeStamp] datetime  NULL,
    [EmpSalaryRecordId] int  NULL,
    [IncrementAmount] float  NULL
);
GO

-- Creating table 'Employee_SalaryRecord'
CREATE TABLE [dbo].[Employee_SalaryRecord] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NULL,
    [StartingSalary] float  NULL,
    [CurrentSalary] float  NULL,
    [TimeStamp] datetime  NULL
);
GO

-- Creating table 'EmployeeAdvanceSalaries'
CREATE TABLE [dbo].[EmployeeAdvanceSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NULL,
    [AdvanceSalary] decimal(18,0)  NULL,
    [Months] nvarchar(50)  NULL,
    [Date] nvarchar(50)  NULL,
    [Status] nvarchar(50)  NULL,
    [SessionId] int  NULL
);
GO

-- Creating table 'EmployeePenalties'
CREATE TABLE [dbo].[EmployeePenalties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NULL,
    [PenaltyId] int  NULL,
    [Month] nvarchar(max)  NULL,
    [Status] nvarchar(max)  NULL,
    [TimeStamp] datetime  NULL
);
GO

-- Creating table 'EmployeePenaltyTypes'
CREATE TABLE [dbo].[EmployeePenaltyTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PenaltyType] varchar(50)  NULL,
    [Amount] float  NULL,
    [TimeStamp] datetime  NULL
);
GO

-- Creating table 'EmployeeSalaries'
CREATE TABLE [dbo].[EmployeeSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NULL,
    [Salary] decimal(18,0)  NULL,
    [Months] nvarchar(50)  NULL,
    [Date] nvarchar(50)  NULL,
    [Status] nvarchar(50)  NULL
);
GO

-- Creating table 'FeeDateSettings'
CREATE TABLE [dbo].[FeeDateSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DueDate] datetime  NULL,
    [ValidityDate] datetime  NULL
);
GO

-- Creating table 'FeeDiscounts'
CREATE TABLE [dbo].[FeeDiscounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NULL
);
GO

-- Creating table 'Ledgers'
CREATE TABLE [dbo].[Ledgers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [StartingBalance] decimal(18,0)  NULL,
    [CurrentBalance] decimal(18,0)  NULL,
    [CreatedBy] nvarchar(256)  NULL,
    [LedgerGroupId] int  NULL,
    [Is_Default] bit  NULL,
    [LedgerHeadId] int  NULL,
    [SessionId] int  NULL
);
GO

-- Creating table 'LedgerGroups'
CREATE TABLE [dbo].[LedgerGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [LedgerHeadID] int  NULL,
    [CreatedBy] nvarchar(256)  NULL
);
GO

-- Creating table 'LedgerHeads'
CREATE TABLE [dbo].[LedgerHeads] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Month_Multiplier'
CREATE TABLE [dbo].[Month_Multiplier] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdjustedMonth] nvarchar(50)  NULL,
    [StudentId] int  NULL,
    [FeeMonthId] int  NULL,
    [PaymentMonth] varchar(50)  NULL
);
GO

-- Creating table 'NonRecurringCharges'
CREATE TABLE [dbo].[NonRecurringCharges] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExpenseType] nvarchar(50)  NULL
);
GO

-- Creating table 'NonRecurringFeeMultipliers'
CREATE TABLE [dbo].[NonRecurringFeeMultipliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentId] int  NULL,
    [DescriptionId] int  NULL,
    [TutionFee] float  NULL,
    [PaidAmount] float  NULL,
    [RemainingAmount] float  NULL,
    [SharePerInstalment] float  NULL,
    [Instalments] int  NULL,
    [PaidInstalments] float  NULL,
    [RemainingInstalments] float  NULL,
    [Multiplier] float  NULL,
    [Month] nvarchar(50)  NULL,
    [Date] nvarchar(50)  NULL,
    [Status] varchar(20)  NULL
);
GO

-- Creating table 'PenaltyFees'
CREATE TABLE [dbo].[PenaltyFees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL,
    [Amount] decimal(18,0)  NULL
);
GO

-- Creating table 'ruffdatas'
CREATE TABLE [dbo].[ruffdatas] (
    [id] int IDENTITY(1,1) NOT NULL,
    [StudentName] nvarchar(200)  NULL,
    [FatherName] nvarchar(200)  NULL,
    [FatherUserName] nvarchar(200)  NULL,
    [FatherPassword] nvarchar(200)  NULL,
    [StudentUserName] nvarchar(200)  NULL,
    [StudentPassword] nvarchar(200)  NULL,
    [StudentClassName] nvarchar(200)  NULL
);
GO

-- Creating table 'Student_ChallanForm'
CREATE TABLE [dbo].[Student_ChallanForm] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassChallanFormId] int  NULL,
    [StudentId] int  NULL,
    [AmountPayable] decimal(18,0)  NULL,
    [TutionFee] decimal(18,0)  NULL
);
GO

-- Creating table 'StudentChallanForms'
CREATE TABLE [dbo].[StudentChallanForms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentId] int  NULL,
    [Status] varchar(50)  NULL,
    [StudentFeeMonthId] int  NULL,
    [ChallanNo] decimal(18,0)  NULL
);
GO

-- Creating table 'StudentDiscounts'
CREATE TABLE [dbo].[StudentDiscounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FeeDiscountId] int  NULL,
    [StudentId] int  NULL,
    [Status] varchar(20)  NULL,
    [Month] varchar(20)  NULL
);
GO

-- Creating table 'StudentFeeMonths'
CREATE TABLE [dbo].[StudentFeeMonths] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentId] int  NULL,
    [Months] nvarchar(50)  NULL,
    [Status] nvarchar(50)  NULL,
    [InstalmentAmount] float  NULL,
    [IssueDate] nvarchar(50)  NULL,
    [DueDate] datetime  NULL,
    [Multiplier] float  NULL,
    [FeePayable] float  NULL,
    [IsPrinted] bit  NULL,
    [SessionId] int  NULL,
    [ValildityDate] datetime  NULL
);
GO

-- Creating table 'StudentPenalties'
CREATE TABLE [dbo].[StudentPenalties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StudentId] int  NULL,
    [PenaltyId] int  NULL,
    [Status] varchar(50)  NULL,
    [Date] varchar(50)  NULL
);
GO

-- Creating table 'StudentPenaltyTypes'
CREATE TABLE [dbo].[StudentPenaltyTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PenaltyType] varchar(50)  NULL,
    [Amount] float  NULL,
    [TimeStamp] datetime  NULL
);
GO

-- Creating table 'StudentRecurringFees'
CREATE TABLE [dbo].[StudentRecurringFees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClassId] int  NULL,
    [ComputerLabCharges] float  NULL,
    [SecurityServices] float  NULL,
    [LabCharges] float  NULL,
    [Transportation] float  NULL,
    [SportsShirt] float  NULL,
    [ChineseClassFee] float  NULL,
    [AdvanceTax] float  NULL,
    [Other] float  NULL,
    [TutionFee] float  NULL,
    [TotalFee] float  NULL,
    [SessionId] int  NULL
);
GO

-- Creating table 'ToDoLists'
CREATE TABLE [dbo].[ToDoLists] (
    [Description] varchar(255)  NULL,
    [CreatedOn] datetime  NULL,
    [Status] varchar(max)  NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserID] nvarchar(128)  NULL
);
GO

-- Creating table 'Vouchers'
CREATE TABLE [dbo].[Vouchers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [Notes] nvarchar(256)  NULL,
    [Date] datetime  NULL,
    [CreatedBy] nvarchar(256)  NULL,
    [VoucherNo] int  NULL
);
GO

-- Creating table 'VoucherRecords'
CREATE TABLE [dbo].[VoucherRecords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LedgerId] int  NULL,
    [Type] nvarchar(256)  NULL,
    [Amount] decimal(18,0)  NULL,
    [CurrentBalance] decimal(18,0)  NULL,
    [AfterBalance] decimal(18,0)  NULL,
    [VoucherId] int  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'AggregatedCounters'
CREATE TABLE [dbo].[AggregatedCounters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Value] bigint  NOT NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'Counters'
CREATE TABLE [dbo].[Counters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Value] smallint  NOT NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'Hashes'
CREATE TABLE [dbo].[Hashes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Field] nvarchar(100)  NOT NULL,
    [Value] nvarchar(max)  NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'Jobs'
CREATE TABLE [dbo].[Jobs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StateId] int  NULL,
    [StateName] nvarchar(20)  NULL,
    [InvocationData] nvarchar(max)  NOT NULL,
    [Arguments] nvarchar(max)  NOT NULL,
    [CreatedAt] datetime  NOT NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'JobParameters'
CREATE TABLE [dbo].[JobParameters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobId] int  NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Value] nvarchar(max)  NULL
);
GO

-- Creating table 'JobQueues'
CREATE TABLE [dbo].[JobQueues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobId] int  NOT NULL,
    [Queue] nvarchar(50)  NOT NULL,
    [FetchedAt] datetime  NULL
);
GO

-- Creating table 'Lists'
CREATE TABLE [dbo].[Lists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Value] nvarchar(max)  NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'Schemata'
CREATE TABLE [dbo].[Schemata] (
    [Version] int  NOT NULL
);
GO

-- Creating table 'Servers'
CREATE TABLE [dbo].[Servers] (
    [Id] nvarchar(100)  NOT NULL,
    [Data] nvarchar(max)  NULL,
    [LastHeartbeat] datetime  NOT NULL
);
GO

-- Creating table 'Sets'
CREATE TABLE [dbo].[Sets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(100)  NOT NULL,
    [Score] float  NOT NULL,
    [Value] nvarchar(256)  NOT NULL,
    [ExpireAt] datetime  NULL
);
GO

-- Creating table 'States'
CREATE TABLE [dbo].[States] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobId] int  NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Reason] nvarchar(100)  NULL,
    [CreatedAt] datetime  NOT NULL,
    [Data] nvarchar(max)  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [EmployeeID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Position] varchar(50)  NULL,
    [Office] varchar(50)  NULL,
    [Age] int  NULL,
    [Salary] int  NULL
);
GO

-- Creating table 'Events'
CREATE TABLE [dbo].[Events] (
    [EventID] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NULL,
    [Subject] nvarchar(100)  NOT NULL,
    [Description] nvarchar(300)  NULL,
    [Start] datetime  NOT NULL,
    [End] datetime  NULL,
    [ThemeColor] nvarchar(10)  NULL,
    [IsFullDay] bit  NOT NULL,
    [IsPublic] bit  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AbsentId] in table 'AspNetAbsent_Student'
ALTER TABLE [dbo].[AspNetAbsent_Student]
ADD CONSTRAINT [PK_AspNetAbsent_Student]
    PRIMARY KEY CLUSTERED ([AbsentId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAnnouncements'
ALTER TABLE [dbo].[AspNetAnnouncements]
ADD CONSTRAINT [PK_AspNetAnnouncements]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAnnouncement_Subject'
ALTER TABLE [dbo].[AspNetAnnouncement_Subject]
ADD CONSTRAINT [PK_AspNetAnnouncement_Subject]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAssessments'
ALTER TABLE [dbo].[AspNetAssessments]
ADD CONSTRAINT [PK_AspNetAssessments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAssessment_Question'
ALTER TABLE [dbo].[AspNetAssessment_Question]
ADD CONSTRAINT [PK_AspNetAssessment_Question]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAssessment_Questions_Category'
ALTER TABLE [dbo].[AspNetAssessment_Questions_Category]
ADD CONSTRAINT [PK_AspNetAssessment_Questions_Category]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAssessment_Topic'
ALTER TABLE [dbo].[AspNetAssessment_Topic]
ADD CONSTRAINT [PK_AspNetAssessment_Topic]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetAttendances'
ALTER TABLE [dbo].[AspNetAttendances]
ADD CONSTRAINT [PK_AspNetAttendances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetCatalogs'
ALTER TABLE [dbo].[AspNetCatalogs]
ADD CONSTRAINT [PK_AspNetCatalogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetChapters'
ALTER TABLE [dbo].[AspNetChapters]
ADD CONSTRAINT [PK_AspNetChapters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetClasses'
ALTER TABLE [dbo].[AspNetClasses]
ADD CONSTRAINT [PK_AspNetClasses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetClass_FeeType'
ALTER TABLE [dbo].[AspNetClass_FeeType]
ADD CONSTRAINT [PK_AspNetClass_FeeType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetCurriculums'
ALTER TABLE [dbo].[AspNetCurriculums]
ADD CONSTRAINT [PK_AspNetCurriculums]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetDurationTypes'
ALTER TABLE [dbo].[AspNetDurationTypes]
ADD CONSTRAINT [PK_AspNetDurationTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetEmp_Auto_Absent'
ALTER TABLE [dbo].[AspNetEmp_Auto_Absent]
ADD CONSTRAINT [PK_AspNetEmp_Auto_Absent]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetEmp_Auto_Attendance'
ALTER TABLE [dbo].[AspNetEmp_Auto_Attendance]
ADD CONSTRAINT [PK_AspNetEmp_Auto_Attendance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetEmployees'
ALTER TABLE [dbo].[AspNetEmployees]
ADD CONSTRAINT [PK_AspNetEmployees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetEmployee_Attendance'
ALTER TABLE [dbo].[AspNetEmployee_Attendance]
ADD CONSTRAINT [PK_AspNetEmployee_Attendance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetEmployeeAttendances'
ALTER TABLE [dbo].[AspNetEmployeeAttendances]
ADD CONSTRAINT [PK_AspNetEmployeeAttendances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFeeChallans'
ALTER TABLE [dbo].[AspNetFeeChallans]
ADD CONSTRAINT [PK_AspNetFeeChallans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFeedBackForms'
ALTER TABLE [dbo].[AspNetFeedBackForms]
ADD CONSTRAINT [PK_AspNetFeedBackForms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFeeTypes'
ALTER TABLE [dbo].[AspNetFeeTypes]
ADD CONSTRAINT [PK_AspNetFeeTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFinanceLedgers'
ALTER TABLE [dbo].[AspNetFinanceLedgers]
ADD CONSTRAINT [PK_AspNetFinanceLedgers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFinanceLedgerTypes'
ALTER TABLE [dbo].[AspNetFinanceLedgerTypes]
ADD CONSTRAINT [PK_AspNetFinanceLedgerTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFinanceMonths'
ALTER TABLE [dbo].[AspNetFinanceMonths]
ADD CONSTRAINT [PK_AspNetFinanceMonths]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetFinancePeriods'
ALTER TABLE [dbo].[AspNetFinancePeriods]
ADD CONSTRAINT [PK_AspNetFinancePeriods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetHoliday_Calendar_Notification'
ALTER TABLE [dbo].[AspNetHoliday_Calendar_Notification]
ADD CONSTRAINT [PK_AspNetHoliday_Calendar_Notification]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetHomeworks'
ALTER TABLE [dbo].[AspNetHomeworks]
ADD CONSTRAINT [PK_AspNetHomeworks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetLessonPlans'
ALTER TABLE [dbo].[AspNetLessonPlans]
ADD CONSTRAINT [PK_AspNetLessonPlans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetLessonPlan_Topic'
ALTER TABLE [dbo].[AspNetLessonPlan_Topic]
ADD CONSTRAINT [PK_AspNetLessonPlan_Topic]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetLessonPlanBreakdowns'
ALTER TABLE [dbo].[AspNetLessonPlanBreakdowns]
ADD CONSTRAINT [PK_AspNetLessonPlanBreakdowns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetLessonPlanBreakdownHeadings'
ALTER TABLE [dbo].[AspNetLessonPlanBreakdownHeadings]
ADD CONSTRAINT [PK_AspNetLessonPlanBreakdownHeadings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetLogs'
ALTER TABLE [dbo].[AspNetLogs]
ADD CONSTRAINT [PK_AspNetLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'AspNetLoginTimes'
ALTER TABLE [dbo].[AspNetLoginTimes]
ADD CONSTRAINT [PK_AspNetLoginTimes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetMessage_Receiver'
ALTER TABLE [dbo].[AspNetMessage_Receiver]
ADD CONSTRAINT [PK_AspNetMessage_Receiver]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetMessages'
ALTER TABLE [dbo].[AspNetMessages]
ADD CONSTRAINT [PK_AspNetMessages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetNotifications'
ALTER TABLE [dbo].[AspNetNotifications]
ADD CONSTRAINT [PK_AspNetNotifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetNotification_User'
ALTER TABLE [dbo].[AspNetNotification_User]
ADD CONSTRAINT [PK_AspNetNotification_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetParents'
ALTER TABLE [dbo].[AspNetParents]
ADD CONSTRAINT [PK_AspNetParents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetParent_Child'
ALTER TABLE [dbo].[AspNetParent_Child]
ADD CONSTRAINT [PK_AspNetParent_Child]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetParent_Notification'
ALTER TABLE [dbo].[AspNetParent_Notification]
ADD CONSTRAINT [PK_AspNetParent_Notification]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetParentTeacherMeetings'
ALTER TABLE [dbo].[AspNetParentTeacherMeetings]
ADD CONSTRAINT [PK_AspNetParentTeacherMeetings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetProjects'
ALTER TABLE [dbo].[AspNetProjects]
ADD CONSTRAINT [PK_AspNetProjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetPTM_ParentFeedback'
ALTER TABLE [dbo].[AspNetPTM_ParentFeedback]
ADD CONSTRAINT [PK_AspNetPTM_ParentFeedback]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetPTM_TeacherFeedback'
ALTER TABLE [dbo].[AspNetPTM_TeacherFeedback]
ADD CONSTRAINT [PK_AspNetPTM_TeacherFeedback]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetPTMAttendances'
ALTER TABLE [dbo].[AspNetPTMAttendances]
ADD CONSTRAINT [PK_AspNetPTMAttendances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetPTMFormRoles'
ALTER TABLE [dbo].[AspNetPTMFormRoles]
ADD CONSTRAINT [PK_AspNetPTMFormRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetPushNotifications'
ALTER TABLE [dbo].[AspNetPushNotifications]
ADD CONSTRAINT [PK_AspNetPushNotifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSalaries'
ALTER TABLE [dbo].[AspNetSalaries]
ADD CONSTRAINT [PK_AspNetSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSalaryDetails'
ALTER TABLE [dbo].[AspNetSalaryDetails]
ADD CONSTRAINT [PK_AspNetSalaryDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSessions'
ALTER TABLE [dbo].[AspNetSessions]
ADD CONSTRAINT [PK_AspNetSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudents'
ALTER TABLE [dbo].[AspNetStudents]
ADD CONSTRAINT [PK_AspNetStudents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Announcement'
ALTER TABLE [dbo].[AspNetStudent_Announcement]
ADD CONSTRAINT [PK_AspNetStudent_Announcement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Assessment]
ADD CONSTRAINT [PK_AspNetStudent_Assessment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Attendance'
ALTER TABLE [dbo].[AspNetStudent_Attendance]
ADD CONSTRAINT [PK_AspNetStudent_Attendance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Attendance_id] in table 'AspNetStudent_AutoAttendance'
ALTER TABLE [dbo].[AspNetStudent_AutoAttendance]
ADD CONSTRAINT [PK_AspNetStudent_AutoAttendance]
    PRIMARY KEY CLUSTERED ([Attendance_id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Fine'
ALTER TABLE [dbo].[AspNetStudent_Fine]
ADD CONSTRAINT [PK_AspNetStudent_Fine]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_HomeWork'
ALTER TABLE [dbo].[AspNetStudent_HomeWork]
ADD CONSTRAINT [PK_AspNetStudent_HomeWork]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Notification'
ALTER TABLE [dbo].[AspNetStudent_Notification]
ADD CONSTRAINT [PK_AspNetStudent_Notification]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Payment'
ALTER TABLE [dbo].[AspNetStudent_Payment]
ADD CONSTRAINT [PK_AspNetStudent_Payment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_PaymentDetail'
ALTER TABLE [dbo].[AspNetStudent_PaymentDetail]
ADD CONSTRAINT [PK_AspNetStudent_PaymentDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Project'
ALTER TABLE [dbo].[AspNetStudent_Project]
ADD CONSTRAINT [PK_AspNetStudent_Project]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Subject'
ALTER TABLE [dbo].[AspNetStudent_Subject]
ADD CONSTRAINT [PK_AspNetStudent_Subject]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Term_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessment]
ADD CONSTRAINT [PK_AspNetStudent_Term_Assessment]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudent_Term_Assessments_Answers'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessments_Answers]
ADD CONSTRAINT [PK_AspNetStudent_Term_Assessments_Answers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetStudentPerformanceReports'
ALTER TABLE [dbo].[AspNetStudentPerformanceReports]
ADD CONSTRAINT [PK_AspNetStudentPerformanceReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSubject_Catalog'
ALTER TABLE [dbo].[AspNetSubject_Catalog]
ADD CONSTRAINT [PK_AspNetSubject_Catalog]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSubject_Homework'
ALTER TABLE [dbo].[AspNetSubject_Homework]
ADD CONSTRAINT [PK_AspNetSubject_Homework]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetSubjects'
ALTER TABLE [dbo].[AspNetSubjects]
ADD CONSTRAINT [PK_AspNetSubjects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetTeachers'
ALTER TABLE [dbo].[AspNetTeachers]
ADD CONSTRAINT [PK_AspNetTeachers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetTerms'
ALTER TABLE [dbo].[AspNetTerms]
ADD CONSTRAINT [PK_AspNetTerms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetTime_Setting'
ALTER TABLE [dbo].[AspNetTime_Setting]
ADD CONSTRAINT [PK_AspNetTime_Setting]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetTopics'
ALTER TABLE [dbo].[AspNetTopics]
ADD CONSTRAINT [PK_AspNetTopics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetVirtualRoles'
ALTER TABLE [dbo].[AspNetVirtualRoles]
ADD CONSTRAINT [PK_AspNetVirtualRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employee_SalaryIncrement'
ALTER TABLE [dbo].[Employee_SalaryIncrement]
ADD CONSTRAINT [PK_Employee_SalaryIncrement]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employee_SalaryRecord'
ALTER TABLE [dbo].[Employee_SalaryRecord]
ADD CONSTRAINT [PK_Employee_SalaryRecord]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeAdvanceSalaries'
ALTER TABLE [dbo].[EmployeeAdvanceSalaries]
ADD CONSTRAINT [PK_EmployeeAdvanceSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeePenalties'
ALTER TABLE [dbo].[EmployeePenalties]
ADD CONSTRAINT [PK_EmployeePenalties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeePenaltyTypes'
ALTER TABLE [dbo].[EmployeePenaltyTypes]
ADD CONSTRAINT [PK_EmployeePenaltyTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalaries'
ALTER TABLE [dbo].[EmployeeSalaries]
ADD CONSTRAINT [PK_EmployeeSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FeeDateSettings'
ALTER TABLE [dbo].[FeeDateSettings]
ADD CONSTRAINT [PK_FeeDateSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FeeDiscounts'
ALTER TABLE [dbo].[FeeDiscounts]
ADD CONSTRAINT [PK_FeeDiscounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Ledgers'
ALTER TABLE [dbo].[Ledgers]
ADD CONSTRAINT [PK_Ledgers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LedgerGroups'
ALTER TABLE [dbo].[LedgerGroups]
ADD CONSTRAINT [PK_LedgerGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LedgerHeads'
ALTER TABLE [dbo].[LedgerHeads]
ADD CONSTRAINT [PK_LedgerHeads]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Month_Multiplier'
ALTER TABLE [dbo].[Month_Multiplier]
ADD CONSTRAINT [PK_Month_Multiplier]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NonRecurringCharges'
ALTER TABLE [dbo].[NonRecurringCharges]
ADD CONSTRAINT [PK_NonRecurringCharges]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NonRecurringFeeMultipliers'
ALTER TABLE [dbo].[NonRecurringFeeMultipliers]
ADD CONSTRAINT [PK_NonRecurringFeeMultipliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PenaltyFees'
ALTER TABLE [dbo].[PenaltyFees]
ADD CONSTRAINT [PK_PenaltyFees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'ruffdatas'
ALTER TABLE [dbo].[ruffdatas]
ADD CONSTRAINT [PK_ruffdatas]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Student_ChallanForm'
ALTER TABLE [dbo].[Student_ChallanForm]
ADD CONSTRAINT [PK_Student_ChallanForm]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentChallanForms'
ALTER TABLE [dbo].[StudentChallanForms]
ADD CONSTRAINT [PK_StudentChallanForms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentDiscounts'
ALTER TABLE [dbo].[StudentDiscounts]
ADD CONSTRAINT [PK_StudentDiscounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentFeeMonths'
ALTER TABLE [dbo].[StudentFeeMonths]
ADD CONSTRAINT [PK_StudentFeeMonths]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentPenalties'
ALTER TABLE [dbo].[StudentPenalties]
ADD CONSTRAINT [PK_StudentPenalties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentPenaltyTypes'
ALTER TABLE [dbo].[StudentPenaltyTypes]
ADD CONSTRAINT [PK_StudentPenaltyTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudentRecurringFees'
ALTER TABLE [dbo].[StudentRecurringFees]
ADD CONSTRAINT [PK_StudentRecurringFees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ToDoLists'
ALTER TABLE [dbo].[ToDoLists]
ADD CONSTRAINT [PK_ToDoLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vouchers'
ALTER TABLE [dbo].[Vouchers]
ADD CONSTRAINT [PK_Vouchers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VoucherRecords'
ALTER TABLE [dbo].[VoucherRecords]
ADD CONSTRAINT [PK_VoucherRecords]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AggregatedCounters'
ALTER TABLE [dbo].[AggregatedCounters]
ADD CONSTRAINT [PK_AggregatedCounters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Counters'
ALTER TABLE [dbo].[Counters]
ADD CONSTRAINT [PK_Counters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Hashes'
ALTER TABLE [dbo].[Hashes]
ADD CONSTRAINT [PK_Hashes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Jobs'
ALTER TABLE [dbo].[Jobs]
ADD CONSTRAINT [PK_Jobs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobParameters'
ALTER TABLE [dbo].[JobParameters]
ADD CONSTRAINT [PK_JobParameters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobQueues'
ALTER TABLE [dbo].[JobQueues]
ADD CONSTRAINT [PK_JobQueues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lists'
ALTER TABLE [dbo].[Lists]
ADD CONSTRAINT [PK_Lists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Version] in table 'Schemata'
ALTER TABLE [dbo].[Schemata]
ADD CONSTRAINT [PK_Schemata]
    PRIMARY KEY CLUSTERED ([Version] ASC);
GO

-- Creating primary key on [Id] in table 'Servers'
ALTER TABLE [dbo].[Servers]
ADD CONSTRAINT [PK_Servers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sets'
ALTER TABLE [dbo].[Sets]
ADD CONSTRAINT [PK_Sets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'States'
ALTER TABLE [dbo].[States]
ADD CONSTRAINT [PK_States]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [EmployeeID] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([EmployeeID] ASC);
GO

-- Creating primary key on [EventID] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [PK_Events]
    PRIMARY KEY CLUSTERED ([EventID] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AnnouncementID] in table 'AspNetAnnouncement_Subject'
ALTER TABLE [dbo].[AspNetAnnouncement_Subject]
ADD CONSTRAINT [FK_Announcement_Subject_ToAnnouncement]
    FOREIGN KEY ([AnnouncementID])
    REFERENCES [dbo].[AspNetAnnouncements]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_Subject_ToAnnouncement'
CREATE INDEX [IX_FK_Announcement_Subject_ToAnnouncement]
ON [dbo].[AspNetAnnouncement_Subject]
    ([AnnouncementID]);
GO

-- Creating foreign key on [AnnouncementID] in table 'AspNetStudent_Announcement'
ALTER TABLE [dbo].[AspNetStudent_Announcement]
ADD CONSTRAINT [FK_Student_Announcement_ToAnnouncement]
    FOREIGN KEY ([AnnouncementID])
    REFERENCES [dbo].[AspNetAnnouncements]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_Announcement_ToAnnouncement'
CREATE INDEX [IX_FK_Student_Announcement_ToAnnouncement]
ON [dbo].[AspNetStudent_Announcement]
    ([AnnouncementID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetAnnouncement_Subject'
ALTER TABLE [dbo].[AspNetAnnouncement_Subject]
ADD CONSTRAINT [FK_Announcement_Subject_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_Subject_ToSubject'
CREATE INDEX [IX_FK_Announcement_Subject_ToSubject]
ON [dbo].[AspNetAnnouncement_Subject]
    ([SubjectID]);
GO

-- Creating foreign key on [AssessmentID] in table 'AspNetAssessment_Topic'
ALTER TABLE [dbo].[AspNetAssessment_Topic]
ADD CONSTRAINT [FK_AspNetAssessment_Topic_ToAssessment]
    FOREIGN KEY ([AssessmentID])
    REFERENCES [dbo].[AspNetAssessments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAssessment_Topic_ToAssessment'
CREATE INDEX [IX_FK_AspNetAssessment_Topic_ToAssessment]
ON [dbo].[AspNetAssessment_Topic]
    ([AssessmentID]);
GO

-- Creating foreign key on [Subject_CatalogID] in table 'AspNetAssessments'
ALTER TABLE [dbo].[AspNetAssessments]
ADD CONSTRAINT [FK_AspNetAssessment_ToSubjectCatalog]
    FOREIGN KEY ([Subject_CatalogID])
    REFERENCES [dbo].[AspNetSubject_Catalog]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAssessment_ToSubjectCatalog'
CREATE INDEX [IX_FK_AspNetAssessment_ToSubjectCatalog]
ON [dbo].[AspNetAssessments]
    ([Subject_CatalogID]);
GO

-- Creating foreign key on [AssessmentID] in table 'AspNetStudent_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Assessment_ToAssessment]
    FOREIGN KEY ([AssessmentID])
    REFERENCES [dbo].[AspNetAssessments]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Assessment_ToAssessment'
CREATE INDEX [IX_FK_AspNetStudent_Assessment_ToAssessment]
ON [dbo].[AspNetStudent_Assessment]
    ([AssessmentID]);
GO

-- Creating foreign key on [QuestionCategory] in table 'AspNetAssessment_Question'
ALTER TABLE [dbo].[AspNetAssessment_Question]
ADD CONSTRAINT [FK_AspNetAssessment_Question_ToQuestionCategory]
    FOREIGN KEY ([QuestionCategory])
    REFERENCES [dbo].[AspNetAssessment_Questions_Category]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAssessment_Question_ToQuestionCategory'
CREATE INDEX [IX_FK_AspNetAssessment_Question_ToQuestionCategory]
ON [dbo].[AspNetAssessment_Question]
    ([QuestionCategory]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetAssessment_Question'
ALTER TABLE [dbo].[AspNetAssessment_Question]
ADD CONSTRAINT [FK_AspNetAssessment_Question_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAssessment_Question_ToSubject'
CREATE INDEX [IX_FK_AspNetAssessment_Question_ToSubject]
ON [dbo].[AspNetAssessment_Question]
    ([SubjectID]);
GO

-- Creating foreign key on [TopicID] in table 'AspNetAssessment_Topic'
ALTER TABLE [dbo].[AspNetAssessment_Topic]
ADD CONSTRAINT [FK_AspNetAssessment_Topic_ToTopic]
    FOREIGN KEY ([TopicID])
    REFERENCES [dbo].[AspNetTopics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAssessment_Topic_ToTopic'
CREATE INDEX [IX_FK_AspNetAssessment_Topic_ToTopic]
ON [dbo].[AspNetAssessment_Topic]
    ([TopicID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetAttendances'
ALTER TABLE [dbo].[AspNetAttendances]
ADD CONSTRAINT [FK_AspNetAttendance_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetAttendance_ToSubject'
CREATE INDEX [IX_FK_AspNetAttendance_ToSubject]
ON [dbo].[AspNetAttendances]
    ([SubjectID]);
GO

-- Creating foreign key on [AttendanceID] in table 'AspNetStudent_Attendance'
ALTER TABLE [dbo].[AspNetStudent_Attendance]
ADD CONSTRAINT [FK_AspNetStudent_Attendance_ToAttendance]
    FOREIGN KEY ([AttendanceID])
    REFERENCES [dbo].[AspNetAttendances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Attendance_ToAttendance'
CREATE INDEX [IX_FK_AspNetStudent_Attendance_ToAttendance]
ON [dbo].[AspNetStudent_Attendance]
    ([AttendanceID]);
GO

-- Creating foreign key on [CatalogID] in table 'AspNetSubject_Catalog'
ALTER TABLE [dbo].[AspNetSubject_Catalog]
ADD CONSTRAINT [FK_AspNetSubject_Catalog_ToCatalog]
    FOREIGN KEY ([CatalogID])
    REFERENCES [dbo].[AspNetCatalogs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSubject_Catalog_ToCatalog'
CREATE INDEX [IX_FK_AspNetSubject_Catalog_ToCatalog]
ON [dbo].[AspNetSubject_Catalog]
    ([CatalogID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetChapters'
ALTER TABLE [dbo].[AspNetChapters]
ADD CONSTRAINT [FK_AspNetChapter_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetChapter_ToSubject'
CREATE INDEX [IX_FK_AspNetChapter_ToSubject]
ON [dbo].[AspNetChapters]
    ([SubjectID]);
GO

-- Creating foreign key on [ChapterID] in table 'AspNetTopics'
ALTER TABLE [dbo].[AspNetTopics]
ADD CONSTRAINT [FK_AspNetTopic_ToSubject]
    FOREIGN KEY ([ChapterID])
    REFERENCES [dbo].[AspNetChapters]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetTopic_ToSubject'
CREATE INDEX [IX_FK_AspNetTopic_ToSubject]
ON [dbo].[AspNetTopics]
    ([ChapterID]);
GO

-- Creating foreign key on [ClassID] in table 'AspNetClass_FeeType'
ALTER TABLE [dbo].[AspNetClass_FeeType]
ADD CONSTRAINT [FK_AspNetClass_FeeType_ToClass]
    FOREIGN KEY ([ClassID])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetClass_FeeType_ToClass'
CREATE INDEX [IX_FK_AspNetClass_FeeType_ToClass]
ON [dbo].[AspNetClass_FeeType]
    ([ClassID]);
GO

-- Creating foreign key on [ClassID] in table 'AspNetSubjects'
ALTER TABLE [dbo].[AspNetSubjects]
ADD CONSTRAINT [FK_AspNetClass_ToAspNetClass]
    FOREIGN KEY ([ClassID])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetClass_ToAspNetClass'
CREATE INDEX [IX_FK_AspNetClass_ToAspNetClass]
ON [dbo].[AspNetSubjects]
    ([ClassID]);
GO

-- Creating foreign key on [TeacherID] in table 'AspNetClasses'
ALTER TABLE [dbo].[AspNetClasses]
ADD CONSTRAINT [FK_AspNetClass_ToAspNetUsers]
    FOREIGN KEY ([TeacherID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetClass_ToAspNetUsers'
CREATE INDEX [IX_FK_AspNetClass_ToAspNetUsers]
ON [dbo].[AspNetClasses]
    ([TeacherID]);
GO

-- Creating foreign key on [ClassID] in table 'AspNetFeeChallans'
ALTER TABLE [dbo].[AspNetFeeChallans]
ADD CONSTRAINT [FK_AspNetFeeChallan_ToClass]
    FOREIGN KEY ([ClassID])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetFeeChallan_ToClass'
CREATE INDEX [IX_FK_AspNetFeeChallan_ToClass]
ON [dbo].[AspNetFeeChallans]
    ([ClassID]);
GO

-- Creating foreign key on [ClassId] in table 'AspNetHomeworks'
ALTER TABLE [dbo].[AspNetHomeworks]
ADD CONSTRAINT [FK_AspNetHomework_AspNetClass]
    FOREIGN KEY ([ClassId])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetHomework_AspNetClass'
CREATE INDEX [IX_FK_AspNetHomework_AspNetClass]
ON [dbo].[AspNetHomeworks]
    ([ClassId]);
GO

-- Creating foreign key on [ClassId] in table 'StudentRecurringFees'
ALTER TABLE [dbo].[StudentRecurringFees]
ADD CONSTRAINT [FK_RecurringFeeClass]
    FOREIGN KEY ([ClassId])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecurringFeeClass'
CREATE INDEX [IX_FK_RecurringFeeClass]
ON [dbo].[StudentRecurringFees]
    ([ClassId]);
GO

-- Creating foreign key on [ClassID] in table 'AspNetStudents'
ALTER TABLE [dbo].[AspNetStudents]
ADD CONSTRAINT [FK_Student_ToAspNetClass]
    FOREIGN KEY ([ClassID])
    REFERENCES [dbo].[AspNetClasses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_ToAspNetClass'
CREATE INDEX [IX_FK_Student_ToAspNetClass]
ON [dbo].[AspNetStudents]
    ([ClassID]);
GO

-- Creating foreign key on [LedgerID] in table 'AspNetClass_FeeType'
ALTER TABLE [dbo].[AspNetClass_FeeType]
ADD CONSTRAINT [FK_AspNetClass_FeeType_ToFeeType]
    FOREIGN KEY ([LedgerID])
    REFERENCES [dbo].[AspNetFinanceLedgers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetClass_FeeType_ToFeeType'
CREATE INDEX [IX_FK_AspNetClass_FeeType_ToFeeType]
ON [dbo].[AspNetClass_FeeType]
    ([LedgerID]);
GO

-- Creating foreign key on [DurationTypeID] in table 'AspNetFeeChallans'
ALTER TABLE [dbo].[AspNetFeeChallans]
ADD CONSTRAINT [FK_AspNetFeeChallan_ToDurationType]
    FOREIGN KEY ([DurationTypeID])
    REFERENCES [dbo].[AspNetDurationTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetFeeChallan_ToDurationType'
CREATE INDEX [IX_FK_AspNetFeeChallan_ToDurationType]
ON [dbo].[AspNetFeeChallans]
    ([DurationTypeID]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeAdvanceSalaries'
ALTER TABLE [dbo].[EmployeeAdvanceSalaries]
ADD CONSTRAINT [FK_AdvanceSalaryUser]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvanceSalaryUser'
CREATE INDEX [IX_FK_AdvanceSalaryUser]
ON [dbo].[EmployeeAdvanceSalaries]
    ([EmployeeId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetEmployees'
ALTER TABLE [dbo].[AspNetEmployees]
ADD CONSTRAINT [FK_AspNetEmployee_AspNetUsers]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetEmployee_AspNetUsers'
CREATE INDEX [IX_FK_AspNetEmployee_AspNetUsers]
ON [dbo].[AspNetEmployees]
    ([UserId]);
GO

-- Creating foreign key on [EmployeeID] in table 'AspNetEmployee_Attendance'
ALTER TABLE [dbo].[AspNetEmployee_Attendance]
ADD CONSTRAINT [FK_AspNetEmployee_Attendance_TO_AspNetEmployee]
    FOREIGN KEY ([EmployeeID])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetEmployee_Attendance_TO_AspNetEmployee'
CREATE INDEX [IX_FK_AspNetEmployee_Attendance_TO_AspNetEmployee]
ON [dbo].[AspNetEmployee_Attendance]
    ([EmployeeID]);
GO

-- Creating foreign key on [VirtualRoleId] in table 'AspNetEmployees'
ALTER TABLE [dbo].[AspNetEmployees]
ADD CONSTRAINT [FK_AspNetEmployee_VirtualRole]
    FOREIGN KEY ([VirtualRoleId])
    REFERENCES [dbo].[AspNetVirtualRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetEmployee_VirtualRole'
CREATE INDEX [IX_FK_AspNetEmployee_VirtualRole]
ON [dbo].[AspNetEmployees]
    ([VirtualRoleId]);
GO

-- Creating foreign key on [EmployeeId] in table 'AspNetSalaryDetails'
ALTER TABLE [dbo].[AspNetSalaryDetails]
ADD CONSTRAINT [FK_AspNetSalaryDetail_AspNetEmployee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSalaryDetail_AspNetEmployee'
CREATE INDEX [IX_FK_AspNetSalaryDetail_AspNetEmployee]
ON [dbo].[AspNetSalaryDetails]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeePenalties'
ALTER TABLE [dbo].[EmployeePenalties]
ADD CONSTRAINT [FK_EmployeePenalty_Employee]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeePenalty_Employee'
CREATE INDEX [IX_FK_EmployeePenalty_Employee]
ON [dbo].[EmployeePenalties]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'Employee_SalaryRecord'
ALTER TABLE [dbo].[Employee_SalaryRecord]
ADD CONSTRAINT [FK_SalaryRecord_EmployeeId]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalaryRecord_EmployeeId'
CREATE INDEX [IX_FK_SalaryRecord_EmployeeId]
ON [dbo].[Employee_SalaryRecord]
    ([EmployeeId]);
GO

-- Creating foreign key on [EmployeeId] in table 'EmployeeSalaries'
ALTER TABLE [dbo].[EmployeeSalaries]
ADD CONSTRAINT [FK_SalaryUser]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[AspNetEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalaryUser'
CREATE INDEX [IX_FK_SalaryUser]
ON [dbo].[EmployeeSalaries]
    ([EmployeeId]);
GO

-- Creating foreign key on [AttendanceID] in table 'AspNetEmployee_Attendance'
ALTER TABLE [dbo].[AspNetEmployee_Attendance]
ADD CONSTRAINT [FK_AspNetEmployee_Attendance_To_AspNetEmployeeAttendance]
    FOREIGN KEY ([AttendanceID])
    REFERENCES [dbo].[AspNetEmployeeAttendances]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetEmployee_Attendance_To_AspNetEmployeeAttendance'
CREATE INDEX [IX_FK_AspNetEmployee_Attendance_To_AspNetEmployeeAttendance]
ON [dbo].[AspNetEmployee_Attendance]
    ([AttendanceID]);
GO

-- Creating foreign key on [FeeChallanID] in table 'AspNetStudent_Payment'
ALTER TABLE [dbo].[AspNetStudent_Payment]
ADD CONSTRAINT [FK_AspNetStudent_Payment_ToFeeChallan]
    FOREIGN KEY ([FeeChallanID])
    REFERENCES [dbo].[AspNetFeeChallans]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Payment_ToFeeChallan'
CREATE INDEX [IX_FK_AspNetStudent_Payment_ToFeeChallan]
ON [dbo].[AspNetStudent_Payment]
    ([FeeChallanID]);
GO

-- Creating foreign key on [FormFor] in table 'AspNetFeedBackForms'
ALTER TABLE [dbo].[AspNetFeedBackForms]
ADD CONSTRAINT [FK_AspNetFeedBackForm_ToRole]
    FOREIGN KEY ([FormFor])
    REFERENCES [dbo].[AspNetPTMFormRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetFeedBackForm_ToRole'
CREATE INDEX [IX_FK_AspNetFeedBackForm_ToRole]
ON [dbo].[AspNetFeedBackForms]
    ([FormFor]);
GO

-- Creating foreign key on [HeadingID] in table 'AspNetPTM_ParentFeedback'
ALTER TABLE [dbo].[AspNetPTM_ParentFeedback]
ADD CONSTRAINT [FK_AspNetPTM_ParentFeedback_ToForm]
    FOREIGN KEY ([HeadingID])
    REFERENCES [dbo].[AspNetFeedBackForms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTM_ParentFeedback_ToForm'
CREATE INDEX [IX_FK_AspNetPTM_ParentFeedback_ToForm]
ON [dbo].[AspNetPTM_ParentFeedback]
    ([HeadingID]);
GO

-- Creating foreign key on [HeadingID] in table 'AspNetPTM_TeacherFeedback'
ALTER TABLE [dbo].[AspNetPTM_TeacherFeedback]
ADD CONSTRAINT [FK_AspNetPTM_TeacherFeedback_ToForm]
    FOREIGN KEY ([HeadingID])
    REFERENCES [dbo].[AspNetFeedBackForms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTM_TeacherFeedback_ToForm'
CREATE INDEX [IX_FK_AspNetPTM_TeacherFeedback_ToForm]
ON [dbo].[AspNetPTM_TeacherFeedback]
    ([HeadingID]);
GO

-- Creating foreign key on [LedgerID] in table 'AspNetFeeTypes'
ALTER TABLE [dbo].[AspNetFeeTypes]
ADD CONSTRAINT [FK_AspNetFeeType_ToLedger]
    FOREIGN KEY ([LedgerID])
    REFERENCES [dbo].[AspNetFinanceLedgers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetFeeType_ToLedger'
CREATE INDEX [IX_FK_AspNetFeeType_ToLedger]
ON [dbo].[AspNetFeeTypes]
    ([LedgerID]);
GO

-- Creating foreign key on [LedgerID] in table 'AspNetStudent_PaymentDetail'
ALTER TABLE [dbo].[AspNetStudent_PaymentDetail]
ADD CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToLedger]
    FOREIGN KEY ([LedgerID])
    REFERENCES [dbo].[AspNetFinanceLedgers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_PaymentDetail_ToLedger'
CREATE INDEX [IX_FK_AspNetStudent_PaymentDetail_ToLedger]
ON [dbo].[AspNetStudent_PaymentDetail]
    ([LedgerID]);
GO

-- Creating foreign key on [PeriodId] in table 'AspNetFinanceMonths'
ALTER TABLE [dbo].[AspNetFinanceMonths]
ADD CONSTRAINT [FK_AspNetFinanceMonth_AspNetFinancePeriod]
    FOREIGN KEY ([PeriodId])
    REFERENCES [dbo].[AspNetFinancePeriods]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetFinanceMonth_AspNetFinancePeriod'
CREATE INDEX [IX_FK_AspNetFinanceMonth_AspNetFinancePeriod]
ON [dbo].[AspNetFinanceMonths]
    ([PeriodId]);
GO

-- Creating foreign key on [HomeworkID] in table 'AspNetStudent_HomeWork'
ALTER TABLE [dbo].[AspNetStudent_HomeWork]
ADD CONSTRAINT [FK_AspNetStudent_HomeWork_AspNetHomework]
    FOREIGN KEY ([HomeworkID])
    REFERENCES [dbo].[AspNetHomeworks]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_HomeWork_AspNetHomework'
CREATE INDEX [IX_FK_AspNetStudent_HomeWork_AspNetHomework]
ON [dbo].[AspNetStudent_HomeWork]
    ([HomeworkID]);
GO

-- Creating foreign key on [LessonPlanID] in table 'AspNetLessonPlan_Topic'
ALTER TABLE [dbo].[AspNetLessonPlan_Topic]
ADD CONSTRAINT [FK_AspNetLessonPlan_Topic_ToLessonPlan]
    FOREIGN KEY ([LessonPlanID])
    REFERENCES [dbo].[AspNetLessonPlans]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetLessonPlan_Topic_ToLessonPlan'
CREATE INDEX [IX_FK_AspNetLessonPlan_Topic_ToLessonPlan]
ON [dbo].[AspNetLessonPlan_Topic]
    ([LessonPlanID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetLessonPlans'
ALTER TABLE [dbo].[AspNetLessonPlans]
ADD CONSTRAINT [FK_AspNetLessonPlan_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetLessonPlan_ToSubject'
CREATE INDEX [IX_FK_AspNetLessonPlan_ToSubject]
ON [dbo].[AspNetLessonPlans]
    ([SubjectID]);
GO

-- Creating foreign key on [LessonPlanID] in table 'AspNetLessonPlanBreakdowns'
ALTER TABLE [dbo].[AspNetLessonPlanBreakdowns]
ADD CONSTRAINT [FK_LessonPlanBreakdown_ToLessonPlan]
    FOREIGN KEY ([LessonPlanID])
    REFERENCES [dbo].[AspNetLessonPlans]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LessonPlanBreakdown_ToLessonPlan'
CREATE INDEX [IX_FK_LessonPlanBreakdown_ToLessonPlan]
ON [dbo].[AspNetLessonPlanBreakdowns]
    ([LessonPlanID]);
GO

-- Creating foreign key on [TopicID] in table 'AspNetLessonPlan_Topic'
ALTER TABLE [dbo].[AspNetLessonPlan_Topic]
ADD CONSTRAINT [FK_AspNetLessonPlan_Topic_ToTopic]
    FOREIGN KEY ([TopicID])
    REFERENCES [dbo].[AspNetTopics]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetLessonPlan_Topic_ToTopic'
CREATE INDEX [IX_FK_AspNetLessonPlan_Topic_ToTopic]
ON [dbo].[AspNetLessonPlan_Topic]
    ([TopicID]);
GO

-- Creating foreign key on [BreakDownHeadingID] in table 'AspNetLessonPlanBreakdowns'
ALTER TABLE [dbo].[AspNetLessonPlanBreakdowns]
ADD CONSTRAINT [FK_LessonPlanBreakdown_ToLessonPlanBreakDownHeading]
    FOREIGN KEY ([BreakDownHeadingID])
    REFERENCES [dbo].[AspNetLessonPlanBreakdownHeadings]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LessonPlanBreakdown_ToLessonPlanBreakDownHeading'
CREATE INDEX [IX_FK_LessonPlanBreakdown_ToLessonPlanBreakDownHeading]
ON [dbo].[AspNetLessonPlanBreakdowns]
    ([BreakDownHeadingID]);
GO

-- Creating foreign key on [UserID] in table 'AspNetLogs'
ALTER TABLE [dbo].[AspNetLogs]
ADD CONSTRAINT [FK_AspNetLog_ToUsers]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetLog_ToUsers'
CREATE INDEX [IX_FK_AspNetLog_ToUsers]
ON [dbo].[AspNetLogs]
    ([UserID]);
GO

-- Creating foreign key on [MessageID] in table 'AspNetMessage_Receiver'
ALTER TABLE [dbo].[AspNetMessage_Receiver]
ADD CONSTRAINT [FK_MessageReceiver_Messages]
    FOREIGN KEY ([MessageID])
    REFERENCES [dbo].[AspNetMessages]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MessageReceiver_Messages'
CREATE INDEX [IX_FK_MessageReceiver_Messages]
ON [dbo].[AspNetMessage_Receiver]
    ([MessageID]);
GO

-- Creating foreign key on [ReceiverID] in table 'AspNetMessage_Receiver'
ALTER TABLE [dbo].[AspNetMessage_Receiver]
ADD CONSTRAINT [FK_MessageReceiver_User]
    FOREIGN KEY ([ReceiverID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MessageReceiver_User'
CREATE INDEX [IX_FK_MessageReceiver_User]
ON [dbo].[AspNetMessage_Receiver]
    ([ReceiverID]);
GO

-- Creating foreign key on [SenderID] in table 'AspNetMessages'
ALTER TABLE [dbo].[AspNetMessages]
ADD CONSTRAINT [FK_MessagesSender_ToAspNetUsers]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MessagesSender_ToAspNetUsers'
CREATE INDEX [IX_FK_MessagesSender_ToAspNetUsers]
ON [dbo].[AspNetMessages]
    ([SenderID]);
GO

-- Creating foreign key on [SenderID] in table 'AspNetNotifications'
ALTER TABLE [dbo].[AspNetNotifications]
ADD CONSTRAINT [FK_AspNetNotification_ToUser]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetNotification_ToUser'
CREATE INDEX [IX_FK_AspNetNotification_ToUser]
ON [dbo].[AspNetNotifications]
    ([SenderID]);
GO

-- Creating foreign key on [NotificationID] in table 'AspNetNotification_User'
ALTER TABLE [dbo].[AspNetNotification_User]
ADD CONSTRAINT [FK_AspNetNotification_User_ToNotification]
    FOREIGN KEY ([NotificationID])
    REFERENCES [dbo].[AspNetNotifications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetNotification_User_ToNotification'
CREATE INDEX [IX_FK_AspNetNotification_User_ToNotification]
ON [dbo].[AspNetNotification_User]
    ([NotificationID]);
GO

-- Creating foreign key on [UserID] in table 'AspNetNotification_User'
ALTER TABLE [dbo].[AspNetNotification_User]
ADD CONSTRAINT [FK_AspNetNotification_User_ToUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetNotification_User_ToUser'
CREATE INDEX [IX_FK_AspNetNotification_User_ToUser]
ON [dbo].[AspNetNotification_User]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'AspNetParents'
ALTER TABLE [dbo].[AspNetParents]
ADD CONSTRAINT [FK_AspNetParent_ToParent]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetParent_ToParent'
CREATE INDEX [IX_FK_AspNetParent_ToParent]
ON [dbo].[AspNetParents]
    ([UserID]);
GO

-- Creating foreign key on [ChildID] in table 'AspNetParent_Child'
ALTER TABLE [dbo].[AspNetParent_Child]
ADD CONSTRAINT [FK_AspNetParent_Child_ChildToUsers]
    FOREIGN KEY ([ChildID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetParent_Child_ChildToUsers'
CREATE INDEX [IX_FK_AspNetParent_Child_ChildToUsers]
ON [dbo].[AspNetParent_Child]
    ([ChildID]);
GO

-- Creating foreign key on [ParentID] in table 'AspNetParent_Child'
ALTER TABLE [dbo].[AspNetParent_Child]
ADD CONSTRAINT [FK_AspNetParent_Child_ParentToUsers]
    FOREIGN KEY ([ParentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetParent_Child_ParentToUsers'
CREATE INDEX [IX_FK_AspNetParent_Child_ParentToUsers]
ON [dbo].[AspNetParent_Child]
    ([ParentID]);
GO

-- Creating foreign key on [ParentID] in table 'AspNetParent_Notification'
ALTER TABLE [dbo].[AspNetParent_Notification]
ADD CONSTRAINT [FK_AspNetParent_Notification_ParentToUser]
    FOREIGN KEY ([ParentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetParent_Notification_ParentToUser'
CREATE INDEX [IX_FK_AspNetParent_Notification_ParentToUser]
ON [dbo].[AspNetParent_Notification]
    ([ParentID]);
GO

-- Creating foreign key on [SenderID] in table 'AspNetParent_Notification'
ALTER TABLE [dbo].[AspNetParent_Notification]
ADD CONSTRAINT [FK_AspNetParent_Notification_SenderToUser]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetParent_Notification_SenderToUser'
CREATE INDEX [IX_FK_AspNetParent_Notification_SenderToUser]
ON [dbo].[AspNetParent_Notification]
    ([SenderID]);
GO

-- Creating foreign key on [MeetingID] in table 'AspNetPTMAttendances'
ALTER TABLE [dbo].[AspNetPTMAttendances]
ADD CONSTRAINT [FK_AspNetPTMAttendance_ToMeeting]
    FOREIGN KEY ([MeetingID])
    REFERENCES [dbo].[AspNetParentTeacherMeetings]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTMAttendance_ToMeeting'
CREATE INDEX [IX_FK_AspNetPTMAttendance_ToMeeting]
ON [dbo].[AspNetPTMAttendances]
    ([MeetingID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetProjects'
ALTER TABLE [dbo].[AspNetProjects]
ADD CONSTRAINT [FK_AspNetProject_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetProject_ToSubject'
CREATE INDEX [IX_FK_AspNetProject_ToSubject]
ON [dbo].[AspNetProjects]
    ([SubjectID]);
GO

-- Creating foreign key on [ProjectID] in table 'AspNetStudent_Project'
ALTER TABLE [dbo].[AspNetStudent_Project]
ADD CONSTRAINT [FK_AspNetStudent_Project_ToProject]
    FOREIGN KEY ([ProjectID])
    REFERENCES [dbo].[AspNetProjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Project_ToProject'
CREATE INDEX [IX_FK_AspNetStudent_Project_ToProject]
ON [dbo].[AspNetStudent_Project]
    ([ProjectID]);
GO

-- Creating foreign key on [PTMID] in table 'AspNetPTM_ParentFeedback'
ALTER TABLE [dbo].[AspNetPTM_ParentFeedback]
ADD CONSTRAINT [FK_AspNetPTM_ParentFeedback_ToPTM]
    FOREIGN KEY ([PTMID])
    REFERENCES [dbo].[AspNetPTMAttendances]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTM_ParentFeedback_ToPTM'
CREATE INDEX [IX_FK_AspNetPTM_ParentFeedback_ToPTM]
ON [dbo].[AspNetPTM_ParentFeedback]
    ([PTMID]);
GO

-- Creating foreign key on [PTMID] in table 'AspNetPTM_TeacherFeedback'
ALTER TABLE [dbo].[AspNetPTM_TeacherFeedback]
ADD CONSTRAINT [FK_AspNetPTM_TeacherFeedback_ToPTM]
    FOREIGN KEY ([PTMID])
    REFERENCES [dbo].[AspNetPTMAttendances]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTM_TeacherFeedback_ToPTM'
CREATE INDEX [IX_FK_AspNetPTM_TeacherFeedback_ToPTM]
ON [dbo].[AspNetPTM_TeacherFeedback]
    ([PTMID]);
GO

-- Creating foreign key on [ParentID] in table 'AspNetPTMAttendances'
ALTER TABLE [dbo].[AspNetPTMAttendances]
ADD CONSTRAINT [FK_AspNetPTMAttendance_ToParent]
    FOREIGN KEY ([ParentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTMAttendance_ToParent'
CREATE INDEX [IX_FK_AspNetPTMAttendance_ToParent]
ON [dbo].[AspNetPTMAttendances]
    ([ParentID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetPTMAttendances'
ALTER TABLE [dbo].[AspNetPTMAttendances]
ADD CONSTRAINT [FK_AspNetPTMAttendance_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetPTMAttendance_ToSubject'
CREATE INDEX [IX_FK_AspNetPTMAttendance_ToSubject]
ON [dbo].[AspNetPTMAttendances]
    ([SubjectID]);
GO

-- Creating foreign key on [UserID] in table 'AspNetPushNotifications'
ALTER TABLE [dbo].[AspNetPushNotifications]
ADD CONSTRAINT [FK_Notifications_ToAspNetUsers]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Notifications_ToAspNetUsers'
CREATE INDEX [IX_FK_Notifications_ToAspNetUsers]
ON [dbo].[AspNetPushNotifications]
    ([UserID]);
GO

-- Creating foreign key on [VirtualRoleID] in table 'AspNetSalaries'
ALTER TABLE [dbo].[AspNetSalaries]
ADD CONSTRAINT [FK_AspNetSalary_VirtualID]
    FOREIGN KEY ([VirtualRoleID])
    REFERENCES [dbo].[AspNetVirtualRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSalary_VirtualID'
CREATE INDEX [IX_FK_AspNetSalary_VirtualID]
ON [dbo].[AspNetSalaries]
    ([VirtualRoleID]);
GO

-- Creating foreign key on [SalaryId] in table 'AspNetSalaryDetails'
ALTER TABLE [dbo].[AspNetSalaryDetails]
ADD CONSTRAINT [FK_AspNetSalaryDetail_SalaryTable]
    FOREIGN KEY ([SalaryId])
    REFERENCES [dbo].[AspNetSalaries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSalaryDetail_SalaryTable'
CREATE INDEX [IX_FK_AspNetSalaryDetail_SalaryTable]
ON [dbo].[AspNetSalaryDetails]
    ([SalaryId]);
GO

-- Creating foreign key on [SessionId] in table 'AspNetStudent_Term_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToSession]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[AspNetSessions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Term_Assessment_ToSession'
CREATE INDEX [IX_FK_AspNetStudent_Term_Assessment_ToSession]
ON [dbo].[AspNetStudent_Term_Assessment]
    ([SessionId]);
GO

-- Creating foreign key on [SessionID] in table 'AspNetTerms'
ALTER TABLE [dbo].[AspNetTerms]
ADD CONSTRAINT [FK_AspNetTerm_ToSession]
    FOREIGN KEY ([SessionID])
    REFERENCES [dbo].[AspNetSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetTerm_ToSession'
CREATE INDEX [IX_FK_AspNetTerm_ToSession]
ON [dbo].[AspNetTerms]
    ([SessionID]);
GO

-- Creating foreign key on [SessionId] in table 'Ledgers'
ALTER TABLE [dbo].[Ledgers]
ADD CONSTRAINT [FK_Ledger_Session]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[AspNetSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ledger_Session'
CREATE INDEX [IX_FK_Ledger_Session]
ON [dbo].[Ledgers]
    ([SessionId]);
GO

-- Creating foreign key on [SessionId] in table 'StudentRecurringFees'
ALTER TABLE [dbo].[StudentRecurringFees]
ADD CONSTRAINT [FK_RecurringFeeClass_Session]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[AspNetSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecurringFeeClass_Session'
CREATE INDEX [IX_FK_RecurringFeeClass_Session]
ON [dbo].[StudentRecurringFees]
    ([SessionId]);
GO

-- Creating foreign key on [SessionId] in table 'StudentFeeMonths'
ALTER TABLE [dbo].[StudentFeeMonths]
ADD CONSTRAINT [FK_StudentFeeMonth_AspNetSession]
    FOREIGN KEY ([SessionId])
    REFERENCES [dbo].[AspNetSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentFeeMonth_AspNetSession'
CREATE INDEX [IX_FK_StudentFeeMonth_AspNetSession]
ON [dbo].[StudentFeeMonths]
    ([SessionId]);
GO

-- Creating foreign key on [StudentId] in table 'NonRecurringFeeMultipliers'
ALTER TABLE [dbo].[NonRecurringFeeMultipliers]
ADD CONSTRAINT [FK_NonRecurring_student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[AspNetStudents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NonRecurring_student'
CREATE INDEX [IX_FK_NonRecurring_student]
ON [dbo].[NonRecurringFeeMultipliers]
    ([StudentId]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudents'
ALTER TABLE [dbo].[AspNetStudents]
ADD CONSTRAINT [FK_Student_ToAspNetUsers]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_ToAspNetUsers'
CREATE INDEX [IX_FK_Student_ToAspNetUsers]
ON [dbo].[AspNetStudents]
    ([StudentID]);
GO

-- Creating foreign key on [StudentId] in table 'StudentChallanForms'
ALTER TABLE [dbo].[StudentChallanForms]
ADD CONSTRAINT [FK_StudentChallan_Student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[AspNetStudents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentChallan_Student'
CREATE INDEX [IX_FK_StudentChallan_Student]
ON [dbo].[StudentChallanForms]
    ([StudentId]);
GO

-- Creating foreign key on [StudentId] in table 'StudentDiscounts'
ALTER TABLE [dbo].[StudentDiscounts]
ADD CONSTRAINT [FK_studentdiscount_student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[AspNetStudents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_studentdiscount_student'
CREATE INDEX [IX_FK_studentdiscount_student]
ON [dbo].[StudentDiscounts]
    ([StudentId]);
GO

-- Creating foreign key on [StudentId] in table 'StudentFeeMonths'
ALTER TABLE [dbo].[StudentFeeMonths]
ADD CONSTRAINT [FK_StudentFeeMonth_student]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[AspNetStudents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentFeeMonth_student'
CREATE INDEX [IX_FK_StudentFeeMonth_student]
ON [dbo].[StudentFeeMonths]
    ([StudentId]);
GO

-- Creating foreign key on [StudentId] in table 'StudentPenalties'
ALTER TABLE [dbo].[StudentPenalties]
ADD CONSTRAINT [FK_StudentPenalty_Stuedent]
    FOREIGN KEY ([StudentId])
    REFERENCES [dbo].[AspNetStudents]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentPenalty_Stuedent'
CREATE INDEX [IX_FK_StudentPenalty_Stuedent]
ON [dbo].[StudentPenalties]
    ([StudentId]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Announcement'
ALTER TABLE [dbo].[AspNetStudent_Announcement]
ADD CONSTRAINT [FK_Student_Announcement_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_Announcement_ToStudent'
CREATE INDEX [IX_FK_Student_Announcement_ToStudent]
ON [dbo].[AspNetStudent_Announcement]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Assessment_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Assessment_ToStudent'
CREATE INDEX [IX_FK_AspNetStudent_Assessment_ToStudent]
ON [dbo].[AspNetStudent_Assessment]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Attendance'
ALTER TABLE [dbo].[AspNetStudent_Attendance]
ADD CONSTRAINT [FK_AspNetStudent_Attendance_ToUser]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Attendance_ToUser'
CREATE INDEX [IX_FK_AspNetStudent_Attendance_ToUser]
ON [dbo].[AspNetStudent_Attendance]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Fine'
ALTER TABLE [dbo].[AspNetStudent_Fine]
ADD CONSTRAINT [FK_AspNetStudent_Fine_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Fine_ToStudent'
CREATE INDEX [IX_FK_AspNetStudent_Fine_ToStudent]
ON [dbo].[AspNetStudent_Fine]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_HomeWork'
ALTER TABLE [dbo].[AspNetStudent_HomeWork]
ADD CONSTRAINT [FK_AspNetStudent_HomeWork_AspNetUsers]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_HomeWork_AspNetUsers'
CREATE INDEX [IX_FK_AspNetStudent_HomeWork_AspNetUsers]
ON [dbo].[AspNetStudent_HomeWork]
    ([StudentID]);
GO

-- Creating foreign key on [SenderID] in table 'AspNetStudent_Notification'
ALTER TABLE [dbo].[AspNetStudent_Notification]
ADD CONSTRAINT [FK_AspNetStudent_Notification_SenderToUser]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Notification_SenderToUser'
CREATE INDEX [IX_FK_AspNetStudent_Notification_SenderToUser]
ON [dbo].[AspNetStudent_Notification]
    ([SenderID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Notification'
ALTER TABLE [dbo].[AspNetStudent_Notification]
ADD CONSTRAINT [FK_AspNetStudent_Notification_StudentToUser]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Notification_StudentToUser'
CREATE INDEX [IX_FK_AspNetStudent_Notification_StudentToUser]
ON [dbo].[AspNetStudent_Notification]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Payment'
ALTER TABLE [dbo].[AspNetStudent_Payment]
ADD CONSTRAINT [FK_AspNetStudent_Payment_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Payment_ToStudent'
CREATE INDEX [IX_FK_AspNetStudent_Payment_ToStudent]
ON [dbo].[AspNetStudent_Payment]
    ([StudentID]);
GO

-- Creating foreign key on [Student_PaymentID] in table 'AspNetStudent_PaymentDetail'
ALTER TABLE [dbo].[AspNetStudent_PaymentDetail]
ADD CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToStudentPayment]
    FOREIGN KEY ([Student_PaymentID])
    REFERENCES [dbo].[AspNetStudent_Payment]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_PaymentDetail_ToStudentPayment'
CREATE INDEX [IX_FK_AspNetStudent_PaymentDetail_ToStudentPayment]
ON [dbo].[AspNetStudent_PaymentDetail]
    ([Student_PaymentID]);
GO

-- Creating foreign key on [PreviousFeeID] in table 'AspNetStudent_PaymentDetail'
ALTER TABLE [dbo].[AspNetStudent_PaymentDetail]
ADD CONSTRAINT [FK_AspNetStudent_PaymentDetail_ToStudentPaymentPrevious]
    FOREIGN KEY ([PreviousFeeID])
    REFERENCES [dbo].[AspNetStudent_Payment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_PaymentDetail_ToStudentPaymentPrevious'
CREATE INDEX [IX_FK_AspNetStudent_PaymentDetail_ToStudentPaymentPrevious]
ON [dbo].[AspNetStudent_PaymentDetail]
    ([PreviousFeeID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Project'
ALTER TABLE [dbo].[AspNetStudent_Project]
ADD CONSTRAINT [FK_AspNetStudent_Project_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Project_ToStudent'
CREATE INDEX [IX_FK_AspNetStudent_Project_ToStudent]
ON [dbo].[AspNetStudent_Project]
    ([StudentID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Subject'
ALTER TABLE [dbo].[AspNetStudent_Subject]
ADD CONSTRAINT [FK_Student_Subject_ToStudent]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_Subject_ToStudent'
CREATE INDEX [IX_FK_Student_Subject_ToStudent]
ON [dbo].[AspNetStudent_Subject]
    ([StudentID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetStudent_Subject'
ALTER TABLE [dbo].[AspNetStudent_Subject]
ADD CONSTRAINT [FK_Student_Subject_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Student_Subject_ToSubject'
CREATE INDEX [IX_FK_Student_Subject_ToSubject]
ON [dbo].[AspNetStudent_Subject]
    ([SubjectID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetStudent_Term_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Term_Assessment_ToSubject'
CREATE INDEX [IX_FK_AspNetStudent_Term_Assessment_ToSubject]
ON [dbo].[AspNetStudent_Term_Assessment]
    ([SubjectID]);
GO

-- Creating foreign key on [TermID] in table 'AspNetStudent_Term_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToTerm]
    FOREIGN KEY ([TermID])
    REFERENCES [dbo].[AspNetTerms]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Term_Assessment_ToTerm'
CREATE INDEX [IX_FK_AspNetStudent_Term_Assessment_ToTerm]
ON [dbo].[AspNetStudent_Term_Assessment]
    ([TermID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudent_Term_Assessment'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessment]
ADD CONSTRAINT [FK_AspNetStudent_Term_Assessment_ToUsers]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Term_Assessment_ToUsers'
CREATE INDEX [IX_FK_AspNetStudent_Term_Assessment_ToUsers]
ON [dbo].[AspNetStudent_Term_Assessment]
    ([StudentID]);
GO

-- Creating foreign key on [STAID] in table 'AspNetStudent_Term_Assessments_Answers'
ALTER TABLE [dbo].[AspNetStudent_Term_Assessments_Answers]
ADD CONSTRAINT [FK_AspNetStudent_Term_Assessments_Answers_ToStudent_Term_Assessment]
    FOREIGN KEY ([STAID])
    REFERENCES [dbo].[AspNetStudent_Term_Assessment]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudent_Term_Assessments_Answers_ToStudent_Term_Assessment'
CREATE INDEX [IX_FK_AspNetStudent_Term_Assessments_Answers_ToStudent_Term_Assessment]
ON [dbo].[AspNetStudent_Term_Assessments_Answers]
    ([STAID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetStudentPerformanceReports'
ALTER TABLE [dbo].[AspNetStudentPerformanceReports]
ADD CONSTRAINT [FK_AspNetStudentPerformanceReport_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudentPerformanceReport_ToSubject'
CREATE INDEX [IX_FK_AspNetStudentPerformanceReport_ToSubject]
ON [dbo].[AspNetStudentPerformanceReports]
    ([SubjectID]);
GO

-- Creating foreign key on [StudentID] in table 'AspNetStudentPerformanceReports'
ALTER TABLE [dbo].[AspNetStudentPerformanceReports]
ADD CONSTRAINT [FK_AspNetStudentPerformanceReport_ToUsers]
    FOREIGN KEY ([StudentID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetStudentPerformanceReport_ToUsers'
CREATE INDEX [IX_FK_AspNetStudentPerformanceReport_ToUsers]
ON [dbo].[AspNetStudentPerformanceReports]
    ([StudentID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetSubject_Catalog'
ALTER TABLE [dbo].[AspNetSubject_Catalog]
ADD CONSTRAINT [FK_AspNetSubject_Catalog_ToSubject]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSubject_Catalog_ToSubject'
CREATE INDEX [IX_FK_AspNetSubject_Catalog_ToSubject]
ON [dbo].[AspNetSubject_Catalog]
    ([SubjectID]);
GO

-- Creating foreign key on [SubjectID] in table 'AspNetSubject_Homework'
ALTER TABLE [dbo].[AspNetSubject_Homework]
ADD CONSTRAINT [FK_AspNetSubject_Homework_AspNetSubjects]
    FOREIGN KEY ([SubjectID])
    REFERENCES [dbo].[AspNetSubjects]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetSubject_Homework_AspNetSubjects'
CREATE INDEX [IX_FK_AspNetSubject_Homework_AspNetSubjects]
ON [dbo].[AspNetSubject_Homework]
    ([SubjectID]);
GO

-- Creating foreign key on [TeacherID] in table 'AspNetSubjects'
ALTER TABLE [dbo].[AspNetSubjects]
ADD CONSTRAINT [FK_AspNetClass_ToAspNetUsersTeacher]
    FOREIGN KEY ([TeacherID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetClass_ToAspNetUsersTeacher'
CREATE INDEX [IX_FK_AspNetClass_ToAspNetUsersTeacher]
ON [dbo].[AspNetSubjects]
    ([TeacherID]);
GO

-- Creating foreign key on [TeacherID] in table 'AspNetTeachers'
ALTER TABLE [dbo].[AspNetTeachers]
ADD CONSTRAINT [FK_Teacher_ToAspNetUsers]
    FOREIGN KEY ([TeacherID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Teacher_ToAspNetUsers'
CREATE INDEX [IX_FK_Teacher_ToAspNetUsers]
ON [dbo].[AspNetTeachers]
    ([TeacherID]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [UserID] in table 'ToDoLists'
ALTER TABLE [dbo].[ToDoLists]
ADD CONSTRAINT [FK_ToDoList_ToAspNetUsers]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ToDoList_ToAspNetUsers'
CREATE INDEX [IX_FK_ToDoList_ToAspNetUsers]
ON [dbo].[ToDoLists]
    ([UserID]);
GO

-- Creating foreign key on [Id] in table 'AspNetVirtualRoles'
ALTER TABLE [dbo].[AspNetVirtualRoles]
ADD CONSTRAINT [FK_AspNetVirtualRole_AspNetVirtualRole]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[AspNetVirtualRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EmpSalaryRecordId] in table 'Employee_SalaryIncrement'
ALTER TABLE [dbo].[Employee_SalaryIncrement]
ADD CONSTRAINT [FK_SalaryIncrement_SalaryRecord]
    FOREIGN KEY ([EmpSalaryRecordId])
    REFERENCES [dbo].[Employee_SalaryRecord]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalaryIncrement_SalaryRecord'
CREATE INDEX [IX_FK_SalaryIncrement_SalaryRecord]
ON [dbo].[Employee_SalaryIncrement]
    ([EmpSalaryRecordId]);
GO

-- Creating foreign key on [PenaltyId] in table 'EmployeePenalties'
ALTER TABLE [dbo].[EmployeePenalties]
ADD CONSTRAINT [FK_EmployeePenalty_Penalty]
    FOREIGN KEY ([PenaltyId])
    REFERENCES [dbo].[EmployeePenaltyTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeePenalty_Penalty'
CREATE INDEX [IX_FK_EmployeePenalty_Penalty]
ON [dbo].[EmployeePenalties]
    ([PenaltyId]);
GO

-- Creating foreign key on [FeeDiscountId] in table 'StudentDiscounts'
ALTER TABLE [dbo].[StudentDiscounts]
ADD CONSTRAINT [FK_studentdiscount_Feedisocunt]
    FOREIGN KEY ([FeeDiscountId])
    REFERENCES [dbo].[FeeDiscounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_studentdiscount_Feedisocunt'
CREATE INDEX [IX_FK_studentdiscount_Feedisocunt]
ON [dbo].[StudentDiscounts]
    ([FeeDiscountId]);
GO

-- Creating foreign key on [LedgerGroupId] in table 'Ledgers'
ALTER TABLE [dbo].[Ledgers]
ADD CONSTRAINT [FK_Ledger_LedgerGroup]
    FOREIGN KEY ([LedgerGroupId])
    REFERENCES [dbo].[LedgerGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ledger_LedgerGroup'
CREATE INDEX [IX_FK_Ledger_LedgerGroup]
ON [dbo].[Ledgers]
    ([LedgerGroupId]);
GO

-- Creating foreign key on [LedgerHeadId] in table 'Ledgers'
ALTER TABLE [dbo].[Ledgers]
ADD CONSTRAINT [FK_Ledger_LedgerHead]
    FOREIGN KEY ([LedgerHeadId])
    REFERENCES [dbo].[LedgerHeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Ledger_LedgerHead'
CREATE INDEX [IX_FK_Ledger_LedgerHead]
ON [dbo].[Ledgers]
    ([LedgerHeadId]);
GO

-- Creating foreign key on [LedgerId] in table 'VoucherRecords'
ALTER TABLE [dbo].[VoucherRecords]
ADD CONSTRAINT [FK_VoucherRecord_Voucher]
    FOREIGN KEY ([LedgerId])
    REFERENCES [dbo].[Ledgers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VoucherRecord_Voucher'
CREATE INDEX [IX_FK_VoucherRecord_Voucher]
ON [dbo].[VoucherRecords]
    ([LedgerId]);
GO

-- Creating foreign key on [LedgerHeadID] in table 'LedgerGroups'
ALTER TABLE [dbo].[LedgerGroups]
ADD CONSTRAINT [FK_LedgerGroup_LedgerHead]
    FOREIGN KEY ([LedgerHeadID])
    REFERENCES [dbo].[LedgerHeads]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LedgerGroup_LedgerHead'
CREATE INDEX [IX_FK_LedgerGroup_LedgerHead]
ON [dbo].[LedgerGroups]
    ([LedgerHeadID]);
GO

-- Creating foreign key on [FeeMonthId] in table 'Month_Multiplier'
ALTER TABLE [dbo].[Month_Multiplier]
ADD CONSTRAINT [FK_FeeMonth]
    FOREIGN KEY ([FeeMonthId])
    REFERENCES [dbo].[StudentFeeMonths]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FeeMonth'
CREATE INDEX [IX_FK_FeeMonth]
ON [dbo].[Month_Multiplier]
    ([FeeMonthId]);
GO

-- Creating foreign key on [DescriptionId] in table 'NonRecurringFeeMultipliers'
ALTER TABLE [dbo].[NonRecurringFeeMultipliers]
ADD CONSTRAINT [FK_NonRecurring_Description]
    FOREIGN KEY ([DescriptionId])
    REFERENCES [dbo].[NonRecurringCharges]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NonRecurring_Description'
CREATE INDEX [IX_FK_NonRecurring_Description]
ON [dbo].[NonRecurringFeeMultipliers]
    ([DescriptionId]);
GO

-- Creating foreign key on [PenaltyId] in table 'StudentPenalties'
ALTER TABLE [dbo].[StudentPenalties]
ADD CONSTRAINT [FK_StudentPenalty_Penalty]
    FOREIGN KEY ([PenaltyId])
    REFERENCES [dbo].[PenaltyFees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentPenalty_Penalty'
CREATE INDEX [IX_FK_StudentPenalty_Penalty]
ON [dbo].[StudentPenalties]
    ([PenaltyId]);
GO

-- Creating foreign key on [StudentFeeMonthId] in table 'StudentChallanForms'
ALTER TABLE [dbo].[StudentChallanForms]
ADD CONSTRAINT [FK_StudentChallan_StudentFeeMonth]
    FOREIGN KEY ([StudentFeeMonthId])
    REFERENCES [dbo].[StudentFeeMonths]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudentChallan_StudentFeeMonth'
CREATE INDEX [IX_FK_StudentChallan_StudentFeeMonth]
ON [dbo].[StudentChallanForms]
    ([StudentFeeMonthId]);
GO

-- Creating foreign key on [VoucherId] in table 'VoucherRecords'
ALTER TABLE [dbo].[VoucherRecords]
ADD CONSTRAINT [FK_VoucherRecord_LedgerId]
    FOREIGN KEY ([VoucherId])
    REFERENCES [dbo].[Vouchers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VoucherRecord_LedgerId'
CREATE INDEX [IX_FK_VoucherRecord_LedgerId]
ON [dbo].[VoucherRecords]
    ([VoucherId]);
GO

-- Creating foreign key on [JobId] in table 'JobParameters'
ALTER TABLE [dbo].[JobParameters]
ADD CONSTRAINT [FK_HangFire_JobParameter_Job]
    FOREIGN KEY ([JobId])
    REFERENCES [dbo].[Jobs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HangFire_JobParameter_Job'
CREATE INDEX [IX_FK_HangFire_JobParameter_Job]
ON [dbo].[JobParameters]
    ([JobId]);
GO

-- Creating foreign key on [JobId] in table 'States'
ALTER TABLE [dbo].[States]
ADD CONSTRAINT [FK_HangFire_State_Job]
    FOREIGN KEY ([JobId])
    REFERENCES [dbo].[Jobs]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HangFire_State_Job'
CREATE INDEX [IX_FK_HangFire_State_Job]
ON [dbo].[States]
    ([JobId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRole]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUser]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUser]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [UserId] in table 'Events'
ALTER TABLE [dbo].[Events]
ADD CONSTRAINT [FK__Event__UserId__52442E1F]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Event__UserId__52442E1F'
CREATE INDEX [IX_FK__Event__UserId__52442E1F]
ON [dbo].[Events]
    ([UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------