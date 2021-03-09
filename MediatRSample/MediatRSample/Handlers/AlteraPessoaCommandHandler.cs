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
    public class AlteraPessoaCommandHandler : IRequestHandler<AlteraPessoaCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Pessoa> _repository;

        public AlteraPessoaCommandHandler(IMediator mediator, IRepository<Pessoa> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<string> Handle(AlteraPessoaCommand request, CancellationToken cancellationToken)
        {
            var pessoa = new Pessoa()
            {
                Id = request.Id,
                Nome = request.Nome,
                Idade = request.Idade,
                Sexo = request.Sexo
            };

            try
            {
                await _repository.Edit(pessoa);
                await _mediator.Publish(new PessoaAlteradaNotification
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome,
                    Idade = pessoa.Idade,
                    Sexo = pessoa.Sexo,
                    IsEfetivado = true
                });

                return await Task.FromResult("Pessoa alterada com sucesso!");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new PessoaAlteradaNotification
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome,
                    Idade = pessoa.Idade,
                    Sexo = pessoa.Sexo,
                    IsEfetivado = false
                });

                await _mediator.Publish(new ErrorNotification { Error = ex.Message, StackTraceError = ex.StackTrace });

                return await Task.FromResult("Ocorreu um erro ao tentar realizar a alteração.");
            }
        }
    }
}