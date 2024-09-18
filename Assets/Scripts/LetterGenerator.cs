using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{
	List<string> firstNames = new List<string>{ "João", "Jéssica", "Bárbara", "Tomás", "Dafne",
											"Jaime", "Sara", "Bruno", "Davi", "Jefferson", "Ana", "Lídia",
											"Micaela", "Ana", "Lourenço", "Olavo", "Sofia", "Luiz",
											"Moisés", "Estêvão", "Henrique", "Rebeca", "Sebastião", "Simone",
											"Miguel", "Angélica", "Marcos", "Allan", "Ricardo", "Helena", "Daniel",
											"Fernanda", "Joaquim", "Lorena" };

	List<string> surnames = new List<string>{ "Pereira", "Silva", "Lima", "Moraes", "Sousa", "Rodrigues", "Oliveira",
											"Martins", "Almeida", "Barbosa", "Gomes", "Santos", "Coelho", "Carvalho",
											"Souza", "Costa", "Schmidt", "Santos", "Muniz", "Fernandes", "Nascimento", "Maciel",
											"Camargo", "Araújo", "Ribeiro", "Batista", "Barros", "Teixeira", "Müller", "Fischer", "Schneider",
											"Carvalho", "Macedo", "Ferreira", "Borges", "Jaeger", "Ziegler" };


	List<string> streetLastWords = new List<string> { "Dor de cabeça", "Febre", "Tosse", "Fadiga", "Dor muscular", "Calafrios", "Dificuldade para respirar", "Dor no peito", 
												  "Náusea", "Vômito", "Diarreia", "Perda de apetite", "Erupção cutânea", "Cansaço excessivo", "Suores noturnos", "Congestão nasal", 
												  "Dor abdominal", "Dores nas articulações", "Tontura", "Perda de olfato", "Confusão mental", "Olhos vermelhos", "Falta de ar", "Dor de garganta" };


	List<string> streetFirstWords = new List<string> { "Enxaqueca e :", "Gripe e :", "Bronquite e :", "Anemia e :", "Fibromialgia e :", "Malária e :", "Asma e :", "Angina e :", 
												  "Gastrite e :", "Vômito agudo e :", "Gastroenterite e :", "Hepatite e :", "Dermatite e :", "Síndrome da fadiga crônica e :", "Tuberculose e :", 
												  "Sinusite e :", "Apendicite e :", "Artrite e :", "Vertigem e :", "COVID-19 e :", "Demência e :", "Conjuntivite e :", "Doença pulmonar obstrutiva crônica (DPOC) e :", "Faringite" };

	List<string> cities  = new List<string> { "Unimed", "Bradesco Saúde", "Amil", "SulAmérica", "Porto Seguro Saúde", "Hapvida", "NotreDame Intermédica", "Prevent Senior", 
											  "Allianz Saúde", "Benevix", "Golden Cross", "São Francisco Saúde", "Bio Saúde", "GreenLine", "Clinipam", "Vitallis", "Samp", 
											  "Mediservice", "Promed", "Trasmontano", "Notredame", "One Health", "Central Nacional Unimed", "NotreDame Seguros", 
											  "Sompo Saúde", "QSaúde", "Seguros Unimed", "Itálica Saúde", "Assim Saúde", "Care Plus", "GNDI", "E-Pharma Saúde", 
											  "Grupo NotreDame", "Plena Saúde", "Plena Clin", "Omint", "Santa Helena Saúde", "Vivest", "Marítima Saúde" };

	List<string> fakeCities = new List<string> { "Individual", "Familiar", "Empresarial", "Coletivo por Adesão", "Ambulatorial", "Hospitalar", "Hospitalar com Obstetrícia", "Odontológico", 
											   "Referência", "Internacional", "Executivo", "Pré-pagamento", "Coparticipação", "Sem Coparticipação", "Premium", "Essencial", 
											   "Flex", "Master", "Pleno", "Básico" };


	int difficulty = 0;
	public float z = 0;

	const int baseFakeValue = 6;

	public void SetDifficulty(int newDifficulty)
	{
		difficulty = newDifficulty;
	}

	public void Generate(Letter letterComponent)
	{
		string firstName = PickRandomFromList(firstNames) + " ";
		string surname = PickRandomFromList(surnames);
		string houseNumber = Mathf.RoundToInt(Random.Range(1, 420)).ToString() + " ";
		string streetFirstWord = PickRandomFromList(streetFirstWords) + " ";
		string streetLastWord = PickRandomFromList(streetLastWords);
		string city = PickRandomFromList(cities);

		EDeliveryType type = Random.Range(0f, 1f) >= .65f ? EDeliveryType.FirstClass : EDeliveryType.SecondClass;

		bool isLetterValid = true;

		int value = Random.Range(1, baseFakeValue + 1 + difficulty);

		//first couple letters guaranteed to be right
		if (LetterManager.instance.scorePennies < 3 && LetterManager.instance.scorePounds < 1)
			value = 0;

		//10 percent chance of an evil letter if the letter is incorrect, overall 1/100 letters are evil
		int evilLetter = Mathf.RoundToInt(Random.Range(1, 10));

		//generate incorrect letter
		if (value >= baseFakeValue)
		{
			isLetterValid = false;

			//if (evilLetter == 1)
			//{
			//letter = MakeEvilLetter();
			//return;
			//}

			switch (value)
			{
				case baseFakeValue:
					//no stamp
					type = EDeliveryType.Missing;
					break;
				case baseFakeValue + 1:
					type = Random.Range(0f, 1f) >= .5f ? EDeliveryType.FakeFirstClass : EDeliveryType.FakeSecondClass;
					//todo
					break;
				case baseFakeValue + 2:
					//wrong name
					{
						int roll = Random.Range(0, 2);
						if (roll == 0)
							firstName = "";
						else
							surname = "";
						break;
					}
				case baseFakeValue + 3:
					//wrong address
					{
						int roll = Random.Range(0, 3);
						if (roll == 0)
							houseNumber = "";
						else if (roll == 1)
							streetFirstWord = "";
						else
							streetLastWord = "";
						break;
					}
				case baseFakeValue + 4:
					//fake city
					city = PickRandomFromList(fakeCities);
					break;
			}

		}

		string address = firstName + surname + "\n" + houseNumber + streetFirstWord + streetLastWord + "\n" + city;
		letterComponent.Initialise(address, isLetterValid, type, z);
		z += .2f;
		if (z > 8.5)
			z = 0;
	}

	//Letter MakeEvilLetter()
	//{
	//	return new Letter();
	//}

	string PickRandomFromList(List<string> list)
	{
		int index = Mathf.RoundToInt(Random.Range(0, list.Count));

		return list[index];
	}
}
