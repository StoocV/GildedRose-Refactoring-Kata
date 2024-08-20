using System.Collections.Generic;
using GildedRoseKata;
using NUnit.Framework;

namespace GildedRoseTests;

public class GildedRoseTest
{
    [Test]
    public void Foo()
    {
        var items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Name, Is.EqualTo("foo"));
    }

    [Test]
    public void StandardItem_SellByDate_Passed_Quality_Degrades_ByOne()
    {
        // Rule: At the end of each day our system lowers both values for every item
        var items = new List<Item> { new() { Name = "foo", SellIn = 7, Quality = 49 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Quality, Is.EqualTo(48));
    }
    
    [Test]
    public void StandardItem_SellByDate_Passed_Quality_Degrades_ByTwo()
    {
        // Rule: Once the sell by date has passed, Quality degrades twice as fast
        var items = new List<Item> { new() { Name = "foo", SellIn = 0, Quality = 49 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Quality, Is.EqualTo(47));
    }
    
    [Test]
    public void AgedBrie_SellByDate_Passed_Quality_Increases_ByOne()
    {
        // Rule: "Aged Brie" actually increases in Quality the older it gets
        var items = new List<Item> { new() { Name = "Aged Brie", SellIn = 0, Quality = 49 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Quality, Is.EqualTo(50));
        Assert.That(items[0].SellIn, Is.EqualTo(-1));
    }
    
    [Test]
    public void Sulfuras_SellByDate_Passed_Quality_Increases_ByTwo()
    {
        // Rule: "Sulfuras", being a legendary item, never has to be sold or decreases in Quality
        // Note the name in code and README are different, test for actual code value
        var items = new List<Item> { new() { Name = "Sulfuras, Hand of Ragnaros", SellIn = 10, Quality = 49 } };
        var app = new GildedRose(items);
        app.UpdateQuality();
        Assert.That(items[0].Quality, Is.EqualTo(49));
        Assert.That(items[0].SellIn, Is.EqualTo(10));
    }
    
    [Test]
    public void BackstagePasses_Results_InCorrect_Quality()
    {
        // Rule: "Backstage passes", like aged brie, increases in Quality as its SellIn value approaches;
        // Rule: Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but
        // Rule: Quality drops to 0 after the concert
        const string passName = "Backstage passes to a TAFKAL80ETC concert";
        var items = new List<Item>
        {
            new() { Name = passName, SellIn = 10, Quality = 49 }, 
            new() { Name = passName, SellIn = 10, Quality = 40 },
            new() { Name = passName, SellIn = 5, Quality = 40 },
            new() { Name = passName, SellIn = 0, Quality = 40 },
        };
        var app = new GildedRose(items);
        app.UpdateQuality();
        
        // Quality should not increase above 50 rule prevents increase to 51 
        Assert.That(items[0].Quality, Is.EqualTo(50));
        Assert.That(items[0].SellIn, Is.EqualTo(9));
        
        Assert.That(items[1].Quality, Is.EqualTo(42));
        Assert.That(items[1].SellIn, Is.EqualTo(9));
        
        Assert.That(items[2].Quality, Is.EqualTo(43));
        Assert.That(items[2].SellIn, Is.EqualTo(4));
        
        Assert.That(items[3].Quality, Is.EqualTo(0));
        Assert.That(items[3].SellIn, Is.EqualTo(-1));
    }
}