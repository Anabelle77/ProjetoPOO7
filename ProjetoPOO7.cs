using System;
using System.Collections.Generic;
using static Vacinas.Program;
namespace Vacinas;

class Program
{
    static List<Cidadao> _cidadaosVacinados = new List<Cidadao>();
    static List<Funcionario> _funcionarios = new List<Funcionario>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Bem-vindo ao sistema de vacinação!");
            Console.WriteLine("Você é um cidadão ou funcionário? (Digite 'c' para cidadão ou 'f' para funcionário)");
            char choice = char.ToLower(Console.ReadKey().KeyChar);
            Console.WriteLine();

            if (choice == 'c')
            {
                CidadaoMenu();
                break;
            }
            else if (choice == 'f')
            {
                FuncionarioMenu();
                break;
            }
            else
            {
                Console.WriteLine("Opção inválida.");
            }
        }
    }

    static void CidadaoMenu()
    {
        Console.Clear();
        Console.WriteLine("Criação de conta:");

        string nome;
        while (true)
        {
            Console.Write("Digite seu nome: ");
            nome = Console.ReadLine();
            try
            {
                nome = CadastroValidator.VerificarNome(nome);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        int idade;
        while (true)
        {
            Console.Write("Digite sua idade: ");
            if (int.TryParse(Console.ReadLine(), out idade))
            {
                try
                {
                    idade = CadastroValidator.VerificarIdade(idade);
                    break;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Por favor, insira um número válido para a idade.");
            }
        }

        string cpf;
        while (true)
        {
            Console.Write("Digite seu CPF: ");
            cpf = Console.ReadLine();
            try
            {
                cpf = CadastroValidator.VerificarCpf(cpf);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Cidadao novoCidadao = new Cidadao(nome, cpf, idade, false);
        _cidadaosVacinados.Add(novoCidadao);

        Console.WriteLine("Login:");
        Console.Write("Digite seu nome: ");
        string loginNome = Console.ReadLine();
        Console.Write("Digite seu CPF: ");
        string loginCpf = Console.ReadLine();

        Cidadao cidadaoLogado = EncontrarCidadao(loginNome, loginCpf);
        if (cidadaoLogado == null)
        {
            Console.WriteLine("Usuário não encontrado. Tente novamente.");
            return;
        }

        Console.WriteLine("Você foi vacinado? (s/n)");
        bool vacinado = char.ToLower(Console.ReadKey().KeyChar) == 's';
        Console.WriteLine();

        Console.WriteLine($"Nome: {cidadaoLogado.Nome}");
        Console.WriteLine($"Idade: {cidadaoLogado.Idade}");
        Console.WriteLine($"CPF: {cidadaoLogado.CPF}");
        Console.WriteLine($"Vacinado: {(vacinado ? "Sim" : "Não")}");

        Console.WriteLine("Deseja ver a lista de cidadãos vacinados? (s/n)");
        if (char.ToLower(Console.ReadKey().KeyChar) == 's')
        {
            MostrarVacinados();
        }
    }

    static void FuncionarioMenu()
    {
        Console.Clear();
        Console.WriteLine("Criação de conta:");
        Console.Write("Digite seu nome: ");
        string nome = Console.ReadLine();
        Console.Write("Digite sua matrícula: ");
        string matricula = Console.ReadLine();
        Console.Write("Digite o CNPJ da empresa: ");
        string cnpj = Console.ReadLine();

        _funcionarios.Add(new Funcionario(nome, matricula, cnpj));

        Console.WriteLine("Login:");
        Console.Write("Digite sua matrícula: ");
        string loginMatricula = Console.ReadLine();

        Funcionario funcionarioLogado = ProcurarFuncionario(loginMatricula);
        if (funcionarioLogado == null)
        {
            Console.WriteLine("Usuário não encontrado. Tente novamente.");
            return;
        }

        Console.WriteLine($"Nome: {funcionarioLogado.Nome}");
        Console.WriteLine($"Matrícula: {funcionarioLogado.Matricula}");
        Console.WriteLine($"CNPJ da empresa: {funcionarioLogado.CNPJ}");

        Console.WriteLine("Deseja adicionar um cidadão à lista de vacinados? (s/n)");
        if (char.ToLower(Console.ReadKey().KeyChar) == 's')
        {
            AdicionarCidadao();
        }

        Console.WriteLine("Deseja ver a lista de cidadãos vacinados? (s/n)");
        if (char.ToLower(Console.ReadKey().KeyChar) == 's')
        {
            MostrarVacinados();
        }
    }

    static void AdicionarCidadao()
    {
        Console.Clear();
        Console.WriteLine("\nAdicionar cidadão à lista de vacinados:");

        string cidadaoNome;
        while (true)
        {
            Console.Write("Digite o nome do cidadão: ");
            cidadaoNome = Console.ReadLine();
            try
            {
                cidadaoNome = CadastroValidator.VerificarNome(cidadaoNome);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        string cidadaoCpf;
        while (true)
        {
            Console.Write("Digite o CPF do cidadão: ");
            cidadaoCpf = Console.ReadLine();
            try
            {
                cidadaoCpf = CadastroValidator.VerificarCpf(cidadaoCpf);
                break;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        _cidadaosVacinados.Add(new Cidadao(cidadaoNome, cidadaoCpf));
        Console.WriteLine($"{cidadaoNome} adicionado à lista de vacinados com sucesso!");
    }

    static void MostrarVacinados()
    {
        Console.Clear();
        Console.WriteLine("\nLista de cidadãos vacinados:");
        foreach (var cidadao in _cidadaosVacinados)
        {
            Console.WriteLine($"Nome: {cidadao.Nome}, CPF: {cidadao.CPF}");
        }
    }

    static Cidadao EncontrarCidadao(string nome, string cpf)
    {
        foreach (var cidadao in _cidadaosVacinados)
        {
            if (cidadao.Nome == nome && cidadao.CPF == cpf)
            {
                return cidadao;
            }
        }
        return null;
    }

    static Funcionario ProcurarFuncionario(string matricula)
    {
        foreach (var funcionario in _funcionarios)
        {
            if (funcionario.Matricula == matricula)
            {
                return funcionario;
            }
        }
        return null;
    }

    public static class CadastroValidator
    {
        public static int VerificarIdade(int idade)
        {
            if (idade > 0 && idade < 200)
            {
                return idade;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(idade), "A idade deve ser um valor positivo e menor que 200.");
            }
        }

        public static string VerificarCpf(string cpf)
        {
            if (ValidarCpf(cpf))
            {
                return cpf;
            }
            else
            {
                throw new ArgumentException("O CPF fornecido não é válido.");
            }
        }

        public static string VerificarCnpj(string cnpj)
        {
            if (ValidarCpf(cnpj))
            {
                return cnpj;
            }
            else
            {
                throw new ArgumentException("O CNPJ fornecido não é válido.");
            }
        }

        public static string VerificarNome(string nome)
        {
            if (ValidarNome(nome))
            {
                return nome;
            }
            else
            {
                throw new ArgumentException("O nome deve conter apenas letras e espaços, e não pode estar vazio.");
            }
        }

        public static bool ValidarCpf(string cpf)
        {
            string cpfNumerico = cpf.Replace(".", "").Replace("-", "");

            if (cpfNumerico.Length == 11 && long.TryParse(cpfNumerico, out _))
            {
                return true;
            }
            return false;
        }

        public static bool ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || !IsOnlyLetters(nome))
            {
                return false;
            }
            return true;
        }


        public static bool ValidarCnpj(string cnpj)
        {
            string cnpjNumerico = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

            if (cnpjNumerico.Length == 14 && long.TryParse(cnpjNumerico, out _))
            {
                return true;
            }
            return false;
        }

    
        public static bool ValidarData(string data, bool isFuturo)
        {
            if (DateTime.TryParse(data, out DateTime dataConvertida))
            {
                DateTime agora = DateTime.Now;
                if (isFuturo)
                {
                    return dataConvertida > agora;
                }
                else
                {
                    return dataConvertida < agora;
                }
            }
            return false;
        }


        private static bool IsOnlyLetters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

class Cidadao
{
    private string _cpf;
    public string Nome { get; }
    public int Idade { get; }
    public string CPF
    {
        get => _cpf;
        private set
        {
            if (CadastroValidator.ValidarCpf(value))
            {
                _cpf = value;
            }
            else
            {
                throw new ArgumentException("O CPF fornecido não é válido.");
            }
        }
    }
    public bool Vacinado { get; }

    public Cidadao(string nome, string cpf, int idade, bool vacinado)
    {
        try
        {
            Nome = CadastroValidator.VerificarNome(nome);
            CPF = CadastroValidator.VerificarCpf(cpf);
            Idade = CadastroValidator.VerificarIdade(idade);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar cidadão: {ex.Message}");
        }
        Vacinado = vacinado;
    }

    public Cidadao(string nome, string cpf) : this(nome, cpf, 0, false) { }
}

class Funcionario
{
    private string _cnpj;
    public string Nome { get; }
    public string Matricula { get; }
    public string CNPJ
    {
        get => _cnpj;
        private set
        {
            if (CadastroValidator.ValidarCnpj(value))
            {
                _cnpj = value;
            }
            else
            {
                throw new ArgumentException("O CNPJ fornecido não é válido.");
            }
        }
    }

    public Funcionario(string nome, string matricula, string cnpj)
    {
        try
        {
            Nome = CadastroValidator.VerificarNome(nome);
            Matricula = matricula;
            CNPJ = CadastroValidator.VerificarCnpj(cnpj);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar funcionário: {ex.Message}");
        }
    }
}
