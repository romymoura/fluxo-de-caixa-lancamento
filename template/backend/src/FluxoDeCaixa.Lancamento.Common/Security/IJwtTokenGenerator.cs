﻿namespace FluxoDeCaixa.Lancamento.Common.Security;

public interface IJwtTokenGenerator
{
    string GenerateToken(IUser user);
}
