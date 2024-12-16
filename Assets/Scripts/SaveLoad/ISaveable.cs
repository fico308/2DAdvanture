public interface ISaveable
{
    /// <summary>
    /// 获取唯一ID, 每个object都需要有一个唯一ID, TODO: 这里有必要单独用一个类来放唯一ID吗?
    /// </summary>
    /// <returns></returns>
    DataDefination GetID();
    /// <summary>
    /// 注册到DataManger
    /// </summary>
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);

    /// <summary>
    /// 注销(死亡时调用)
    /// </summary>
    void UnregisterSaveData() => DataManager.instance.UnregisterSaveData(this);

    /// <summary>
    ///  保存数据到storage
    /// </summary>
    /// <param name="storage">存储的地方</param>
    void Save(SaveData storage);

    /// <summary>
    /// 从storage中取数据
    /// </summary>
    /// <param name="storage">存储的地方</param>
    void Load(SaveData storage);
}