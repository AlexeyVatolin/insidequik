using System.ComponentModel;

namespace Common.Models
{
    public enum ErrorCodes
    {
        [Description("INVALID_JSON_SYNTAX")]
        CantConvertToJsonObject,
        [Description("INVALID_LOGIN_OR_PASSWORD")]
        WrongUserNameOrPassword,
        [Description("YOU_ARE_NOT_LOGGED_IN")]
        NotLogged,
        [Description("USER_IS_INACTIVE")]
        UserIsNotActive,
        [Description("INVALID_REQUEST_PARAMETERS")]
        RequestIsNotValid,
        [Description("CAN_NOT_FIND_PARAMETER")]
        ParameterNotFound,
        [Description("CAN_NOT_FIND_USER")]
        UserNotFound,
        [Description("PARAMETER_IS_NULL_OR_EMPTY")]
        ParameterIsNullOrEmpty,
        [Description("USER_IS_FREEZED")]
        UserIsFreezed
    }
}
