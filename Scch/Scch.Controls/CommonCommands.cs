using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Scch.Controls.Properties;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls
{
    /// <summary>
    /// Creates commonly used <see cref="CommandViewModel"/>.
    /// </summary>
    public static class CommonCommands
    {
        /// <summary>
        /// <see cref="ICommandViewModel"/> for "start".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateStartCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_StartDisplayName, Resources.CommonCommands_StartToolTip, command, true,
                Resources.Start_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "screenshot".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateScreenshotCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_ScreenshotDisplayName, Resources.CommonCommands_ScreenshotToolTip, command,
                Resources.Screenshot_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "picture".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePictureCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PictureDisplayName, Resources.CommonCommands_PictureToolTip, command,
                Resources.Picture_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "play".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePlayCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PlayDisplayName, Resources.CommonCommands_PlayToolTip, command,
                true, Resources.Play_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "pause".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePauseCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PauseDisplayName, Resources.CommonCommands_PauseToolTip, command,
                Resources.Pause_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "stop".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateStopCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_StopDisplayName, Resources.CommonCommands_StopToolTip, command,
                Resources.Stop_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "record".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRecordCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RecordDisplayName, Resources.CommonCommands_RecordToolTip, command, 
                CommonShortcuts.CreateRecordShortcut(), true, Resources.Record_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "first".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateFirstCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_FirstDisplayName, Resources.CommonCommands_FirstToolTip, command,
                Resources.First_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "last".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateLastCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_LastDisplayName, Resources.CommonCommands_LastToolTip, command,
                Resources.Last_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "previous".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePreviousCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PreviousDisplayName, Resources.CommonCommands_PreviousToolTip, command,
                Resources.Previous_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "next".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNextCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NextDisplayName, Resources.CommonCommands_NextToolTip, command,
                Resources.Next_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "add new".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateAddNewCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_AddNewDisplayName, Resources.CommonCommands_AddNewToolTip, command,
               CommonShortcuts.CreateAddNewShortcut(), Resources.AddNew_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "delete".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateDeleteCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_DeleteDisplayName, Resources.CommonCommands_DeleteToolTip, command,
                CommonShortcuts.CreateDeleteShortcut(), Resources.Delete_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "calendar".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateCalendarCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_CalendarDisplayName, Resources.CommonCommands_CalendarToolTip, command,
                Resources.Calendar_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "ok".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateOkCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_OkDisplayName, Resources.CommonCommands_OkToolTip, command,
                CommonShortcuts.CreateOkShortcut(), Resources.Ok_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "cancel".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateCancelCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_CancelDisplayName, Resources.CommonCommands_CancelToolTip, command,
                CommonShortcuts.CreateCancelShortcut(), Resources.Cancel_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "favorites".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateFavoritesCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_FavoritesDisplayName, Resources.CommonCommands_FavoritesToolTip, command,
                Resources.Favorites_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "edit".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateEditCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_EditDisplayName, Resources.CommonCommands_EditToolTip, command,
                CommonShortcuts.CreateEditShortcut(), Resources.Edit_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "rename".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRenameCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RenameDisplayName, Resources.CommonCommands_RenameToolTip, command,
                CommonShortcuts.CreateRenameShortcut(), Resources.Rename_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "redo".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRedoCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RedoDisplayName, Resources.CommonCommands_RedoToolTip, command,
                CommonShortcuts.CreateRedoShortcut(), Resources.Redo_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "undo".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateUndoCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_UndoDisplayName, Resources.CommonCommands_UndoToolTip, command,
                CommonShortcuts.CreateUndoShortcut(), Resources.Undo_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "refresh".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRefreshCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RefreshDisplayName, Resources.CommonCommands_RefreshToolTip, command,
                CommonShortcuts.CreateRefreshShortcut(), Resources.Refresh_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "zoom in".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateZoomInCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_ZoomInDisplayName, Resources.CommonCommands_ZoomInToolTip, command,
                CommonShortcuts.CreateZoomInShortcut(), Resources.ZoomIn_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "zoom out".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateZoomOutCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_ZoomOutDisplayName, Resources.CommonCommands_ZoomOutToolTip, command,
                CommonShortcuts.CreateZoomOutShortcut(), Resources.ZoomOut_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "copy".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateCopyCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_CopyDisplayName, Resources.CommonCommands_CopyToolTip, command,
                CommonShortcuts.CreateCopyShortcut(), Resources.Copy_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "cut".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateCutCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_CutDisplayName, Resources.CommonCommands_CutToolTip, command,
                CommonShortcuts.CreateCutShortcut(), Resources.Cut_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "paste".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePasteCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PasteDisplayName, Resources.CommonCommands_PasteToolTip, command,
                CommonShortcuts.CreatePasteShortcut(), Resources.Paste_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "find".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateFindCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_FindDisplayName, Resources.CommonCommands_FindToolTip, command,
                CommonShortcuts.CreateFindShortcut(), Resources.Find_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "search".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateSearchCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_SearchDisplayName, Resources.CommonCommands_SearchToolTip, command,
                CommonShortcuts.CreateSearchShortcut(), Resources.Search_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "folder".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateFolderCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_FolderDisplayName, Resources.CommonCommands_FolderToolTip, command,
                Resources.Folder_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "forward mail".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateForwardMailCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_ForwardMailDisplayName, Resources.CommonCommands_ForwardMailToolTip, command,
                CommonShortcuts.CreateForwardMailShortcut(), Resources.ForwardMail_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "send mail".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateSendMailCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_SendMailDisplayName, Resources.CommonCommands_SendMailToolTip, command,
                Resources.SendMail_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "new mail".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNewMailCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NewMailDisplayName, Resources.CommonCommands_NewMailToolTip, command,
                CommonShortcuts.CreateNewMailShortcut(), Resources.NewMail_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "upload".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateUploadCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_UploadDisplayName, Resources.CommonCommands_UploadToolTip, command,
                Resources.Upload_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "download".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateDownloadCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_DownloadDisplayName, Resources.CommonCommands_DownloadToolTip, command,
                Resources.Download_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "internet".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInternetCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InternetDisplayName, Resources.CommonCommands_InternetToolTip, command,
                Resources.Internet_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "help".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateHelpCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_HelpDisplayName, Resources.CommonCommands_HelpToolTip, command,
                CommonShortcuts.CreateHelpShortcut(), Resources.Help_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "information".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInformationCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InformationDisplayName, Resources.CommonCommands_InformationToolTip, command,
                Resources.Information_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "login".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateLoginCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_LoginDisplayName, Resources.CommonCommands_LoginToolTip, command,
                Resources.Login_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "logout".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateLogoutCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_LogoutDisplayName, Resources.CommonCommands_LogoutToolTip, command,
                Resources.Logout_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "navigate back".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNavigateBackCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NavigateBackDisplayName, Resources.CommonCommands_NavigateBackToolTip, command,
                Resources.NavigateBack_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "navigate forward".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNavigateForwardCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NavigateForwardDisplayName, Resources.CommonCommands_NavigateForwardToolTip, command,
                Resources.NavigateForward_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "new".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNewCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NewDisplayName, Resources.CommonCommands_NewToolTip, command,
                CommonShortcuts.CreateNewShortcut(), Resources.New_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "open".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateOpenCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_OpenDisplayName, Resources.CommonCommands_OpenToolTip, command,
                CommonShortcuts.CreateOpenShortcut(), Resources.Open_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "save".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateSaveCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_SaveDisplayName, Resources.CommonCommands_SaveToolTip, command,
                CommonShortcuts.CreateSaveShortcut(), Resources.Save_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "preview".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePreviewCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PreviewDisplayName, Resources.CommonCommands_PreviewToolTip, command,
                CommonShortcuts.CreatePreviewShortcut(), Resources.Preview_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "print".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePrintCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PrintDisplayName, Resources.CommonCommands_PrintToolTip, command,
                CommonShortcuts.CreatePrintShortcut(), Resources.Print_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "properties".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreatePropertiesCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_PropertiesDisplayName, Resources.CommonCommands_PropertiesToolTip, command,
                CommonShortcuts.CreatePropertiesShortcut(), Resources.Properties_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "settings".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateSettingsCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_SettingsDisplayName, Resources.CommonCommands_SettingsToolTip, command,
                Resources.Settings_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "synchronize".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateSynchronizeCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_SynchronizeDisplayName, Resources.CommonCommands_SynchronizeToolTip, command,
                Resources.Synchronize_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for a separator.
        /// </summary>
        /// <returns></returns>
        public static ICommandViewModel CreateSeparatorCommand()
        {
            return new CommandViewModel("", "", new RelayCommand(DoNothing));
        }

        private static void DoNothing()
        {
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "table".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateTableCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_TableDisplayName, Resources.CommonCommands_TableToolTip, command,
                Resources.Table_32x32);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "line".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateLineCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_LineDisplayName, Resources.CommonCommands_LineToolTip, command,
                Resources.Line_32x32);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "numbering".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateNumberingCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_NumberingDisplayName, Resources.CommonCommands_NumberingToolTip, command,
                CommonShortcuts.CreateNumberingShortcut(), Resources.Numbering_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "bullets".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateBulletsCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_BulletsDisplayName, Resources.CommonCommands_BulletsToolTip, command,
                CommonShortcuts.CreateBulletsShortcut(), Resources.Bullets_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "insert row below".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInsertRowBelowCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InsertRowBelowDisplayName, Resources.CommonCommands_InsertRowBelowToolTip, command,
                Resources.InsertRowBelow_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "insert row above".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInsertRowAboveCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InsertRowAboveDisplayName, Resources.CommonCommands_InsertRowAboveToolTip, command,
                Resources.InsertRowAbove_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "insert column left".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInsertColumnLeftCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InsertColumnLeftDisplayName, Resources.CommonCommands_InsertColumnLeftToolTip, command,
                Resources.InsertColumnLeft_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "insert column right".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateInsertColumnRightCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_InsertColumnRightDisplayName, Resources.CommonCommands_InsertColumnRightToolTip, command,
                Resources.InsertColumnLeft_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "delete row".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateDeleteRowCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_DeleteRowDisplayName, Resources.CommonCommands_DeleteRowToolTip, command,
                Resources.DeleteRow_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "delete column".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateDeleteColumnCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_DeleteColumnDisplayName, Resources.CommonCommands_DeleteColumnToolTip, command,
                Resources.DeleteColumn_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "delete table".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateDeleteTableCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_DeleteTableDisplayName, Resources.CommonCommands_DeleteTableToolTip, command,
                Resources.DeleteTable_16x16);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "add".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateAddCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_AddDisplayName, Resources.CommonCommands_AddToolTip, command,
                Resources.Add_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "add all".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateAddAllCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_AddAllDisplayName, Resources.CommonCommands_AddAllToolTip, command,
                Resources.AddAll_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "remove".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRemoveCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RemoveDisplayName, Resources.CommonCommands_RemoveToolTip, command,
                Resources.Remove_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "remove all".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateRemoveAllCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_RemoveAllDisplayName, Resources.CommonCommands_RemoveAllToolTip, command,
                Resources.RemoveAll_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "move bottom".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateMoveBottomCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_MoveBottomDisplayName, Resources.CommonCommands_MoveBottomToolTip, command,
                Resources.MoveBottom_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "move top".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateMoveTopCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_MoveTopDisplayName, Resources.CommonCommands_MoveTopToolTip, command,
                Resources.MoveTop_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "move up".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateMoveUpCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_MoveUpDisplayName, Resources.CommonCommands_MoveUpToolTip, command,
                CommonShortcuts.CreateMoveUpShortcut(), Resources.MoveUp_64x64);
        }

        /// <summary>
        /// <see cref="ICommandViewModel"/> for "move down".
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static ICommandViewModel CreateMoveDownCommand(ICommand command)
        {
            return new CommandViewModel(Resources.CommonCommands_MoveDownDisplayName, Resources.CommonCommands_MoveDownToolTip, command,
                CommonShortcuts.CreateMoveDownShortcut(), Resources.MoveDown_64x64);
        }
    }
}
