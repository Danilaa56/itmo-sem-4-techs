// using GenAlg.OneMax;
//
// namespace GenAlg;
//
// public class Individual : IIndividual<Individual>
// {
//     public readonly Genome Genome;
//
//     public Individual()
//     {
//         Genome = new Genome();
//     }
//
//     public Individual(Genome genome)
//     {
//         Genome = genome;
//     }
//
//     public Individual Clone()
//     {
//         return new Individual(Genome.Clone());
//     }
//
//     public (Individual, Individual) Cross(Individual other)
//     {
//         var genes = Genome.Cross(other.Genome);
//         return (new Individual(genes.Item1), new Individual(genes.Item2));
//     }
// }