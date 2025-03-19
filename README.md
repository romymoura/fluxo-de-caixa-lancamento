# Teste GFT Fluxo de caixa.

Esse repositório tem como objetivo versionar o projeto afim de fazer o fluxo de caixa.


## Infra do projeto, você deve fazer essas configurações antes de tentar rodar o projeto:

O serviço foi arquitetado para rodar na nuvem, a infra estrutura escrita em terraform você encontra [aqui](https://github.com/romymoura/fluxo-de-caixa-infra)


## API Rest Full, Serviço de lançamento (Débito e Crédito) -> ECS:

* Navegue pelas as especificações e entenda o que foi feito.

	0 - [Desafio](/.doc/0000-desafio.md)

	1 - [Proposta da arquitetura](/.doc/0001-proposta-arquitetura-controle-de-caixa-EDA.md)
 
	2 - [Modelagem](/.doc/0002-modelagem-banco-de-dados.md)
 
	3 - [Sobre o projeto](/.doc/0003-especificações-do-projeto.md)
 
	4 - [Setup do projeto](/.doc/0004-faça-você-mesmo-o-setup.md)




## Worker Consolidador de saldo -> ECS

O serviço que vai consolidar o saldo do caixa você encontra [aqui](https://github.com/romymoura/fluxo-de-caixa-consolidacao)