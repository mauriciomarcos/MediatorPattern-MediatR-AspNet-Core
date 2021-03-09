using MediatR;
using MediatRSample.Commands;
using MediatRSample.Models;
using MediatRSample.Models.Interfaces;
using MediatRSample.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRSample.Handlers
{
    public class CadastraPessoaCommandHandler : IRequestHandler<CadastraPessoaCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Pessoa> _repository;

        public CadastraPessoaCommandHandler(IRepository<Pessoa> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<string> Handle(CadastraPessoaCommand request, CancellationToken cancellationToken)
        {
            var pessoa = new Pessoa
            {
                Nome = request.Nome,
                Idade = request.Idade,
                Sexo = request.Sexo
            };

            try
            {
                await _repository.Add(pessoa);
                await _mediator.Publish(new PessoaCriadaNotification
                {
                    Id = pessoa.Id,
                    Idade = pessoa.Idade,
                    Nome = pessoa.Nome,
                    Sexo = pessoa.Sexo
                });

                return await Task.FromResult("Pessoa criada com sucesso!");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new PessoaCriadaNotification
                {
                    Id = pessoa.Id,
                    Idade = pessoa.Idade,
                    Nome = pessoa.Nome,
                    Sexo = pessoa.Sexo
                });

                await _mediator.Publish(new ErrorNotification
                {
                    Error = ex.Message,
                    StackTraceError = ex.StackTrace
                });

                return await Task.FromResult("Ocorreu umm erro no cadastramento.");
            }
        }
    }
}
