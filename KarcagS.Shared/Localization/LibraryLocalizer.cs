using KarcagS.Shared.Helpers;
using Microsoft.Extensions.Localization;

namespace KarcagS.Shared.Localization;

public class LibraryLocalizer
{
    private static LibraryLocalizer? _instance;
    private IStringLocalizer Localizer { get; set; } = default!;

    private LibraryLocalizer()
    {
    }

    public static LibraryLocalizer GetInstance()
    {
        _instance ??= new LibraryLocalizer();

        return _instance;
    }

    public void AddLocalizer(IStringLocalizer localizer) => Localizer = localizer;

    public bool IsRegistered() => ObjectHelper.IsNotNull(Localizer);

    public string GetValue(string key, params string[] args) => Localizer[key, args];

    public const string DialogCancelKey = "Dialog.Cancel";
    public const string DialogSaveKey = "Dialog.Save";

    public const string UploaderUploadKey = "Uploader.Upload";
    public const string UploaderUploadingErrorKey = "Uploader.Message.UploadingError";
    public const string UploaderInvalidExtensionsKey = "Uploader.Message.InvalidExtensions";
    public const string UploaderInvalidSizeKey = "Uploader.Message.InvalidSize";

    public const string ConfirmConfirmKey = "Confirm.Confirm";
    public const string ConfirmCancelKey = "Confirm.Cancel";
    public const string ConfirmYesKey = "Confirm.Yes";
    public const string ConfirmNoKey = "Confirm.No";

    public const string EditorCancelKey = "Editor.Cancel";
    public const string EditorSaveKey = "Editor.Save";
    public const string EditorRemoveKey = "Editor.Remove";
    public const string EditorEditEntityKey = "Editor.EditEntity";
    public const string EditorCreateEntityKey = "Editor.CreateEntity";
    public const string EditorConfirmDeleteTitleKey = "Editor.ConfirmDelete.Title";
    public const string EditorConfirmDeleteMessageKey = "Editor.ConfirmDelete.Message";

    public const string SelectorSelectEntityKey = "Selector.SelectEntity";
    public const string SelectorSelectActionKey = "Selector.Action.Select";
    public const string SelectorCancelActionKey = "Selector.Action.Cancel";
    public const string SelectorValuesColumnHeaderKey = "Selector.ValuesColumnHeader";

    public const string TableSearchLabelKey = "Table.Search";
}