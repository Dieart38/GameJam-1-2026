using UnityEngine;

// Isso aqui é o segredo: ele cria uma opção no menu do Unity!
[CreateAssetMenu(fileName = "Novo Item", menuName = "Loja/Item")]
public class ItemLoja : ScriptableObject
{
    public string nome;
    [TextArea] public string descricao;
    public int preco;
    public Sprite icone;
    
    public enum TipoItem { Vida, Barreira, PuloDuplo }
    public TipoItem tipo;
}