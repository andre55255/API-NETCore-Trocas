namespace ExchangeApp.Helpers
{
    public static class ConstantsMessagesUser
    {
        public static string ErrorLogin = "Falha ao realizar login de usuário: ";
        public static string ErrorUserNotFound = "Usuário não encontrado: ";
        public static string ErrorValidateLogin = "Falha ao fazer validação de dados de login: ";
        public static string ErrorAttemptsLoginExceeded = "Tentativas de login excedidas, recupere sua senha e tente novamente";
        public static string ErrorAccountBlocked = "Esta conta está bloqueada, recupere a senha ou aguarde a data de desbloqueio: ";
        public static string ErrorExceptionAuth = "Falha na autenticação de usuário: ";
        public static string ErrorCredentialsIncorrect = "Falha de autenticação, senha incorreta";
        public static string ErrorUpdateUserBlockAttempts = "Falha ao atualizar tentativas de login de usuário: ";
        public static string ErrorFindRolesByUser = "Falha ao listar roles de usuário: ";
        public static string ErrorBuildClaimsJwt = "Falha ao gerar claims de token jwt para usuário: ";
        public static string ErrorCreateTokenJwt = "Falha ao gerar token de acesso: ";
        public static string ErrorInvalidToken = "Token de acesso inválido: ";
        public static string ErrorGenerateTokenRefresh = "Falha ao gerar refresh token para usuário: ";
        public static string ErrorFindTokenRefresh = "Falha ao buscar refresh token salvo para usuário: ";
        public static string ErrorRefreshTokenIncorrect = "Refresh token diferente do existente na base de dados para usuário: ";
        public static string ErrorRefreshToken = "Falha ao realizar refresh token";

        public static string SuccessLogin = "Login realizado com sucesso, usuário: ";
        public static string SuccessRefreshToken = "Refresh token realizado com sucesso";
    }
}
