using System;
using System.Collections.Generic;

[Serializable]
public readonly struct GridGraph {
    private readonly Point[,] points;
    private readonly int width;
    private readonly int height;

    public GridGraph(int width, int height) {
        this.points = new Point[height, width];
        this.width = width;
        this.height = height;
    }

    public IEnumerable<(int x, int y)> GetConnectedPoints(int x, int y) {
        if (IsOut(x, y)) {
            yield break;
        }

        if (HasEdgeAroundPointInternal(x, y, EdgeDirection.Up)) {
            yield return (x, y + 1);
        }

        if (HasEdgeAroundPointInternal(x, y, EdgeDirection.Right)) {
            yield return (x + 1, y);
        }

        if (HasEdgeAroundPointInternal(x, y, EdgeDirection.Down)) {
            yield return (x, y - 1);
        }

        if (HasEdgeAroundPointInternal(x, y, EdgeDirection.Left)) {
            yield return (x - 1, y);
        }
    }

    public bool HasEdgeAroundPoint(int x, int y, EdgeDirection direction) {
        return !IsOut(x, y) && HasEdgeAroundPointInternal(x, y, direction);
    }

    public void AddEdgeAroundPoint(int x, int y, EdgeDirection direction) {
        SetEdgeAroundPoint(x, y, direction, edge: true);
    }

    public void RemoveEdgeAroundPoint(int x, int y, EdgeDirection direction) {
        SetEdgeAroundPoint(x, y, direction, edge: false);
    }

    public void SetEdgeAroundPoint(int x, int y, EdgeDirection direction, bool edge) {
        if (IsOut(x, y)) {
            return;
        }

        switch (direction) {
            case EdgeDirection.Up:
                if ((y + 1) < this.height) {
                    this.points[y, x].upEdge = edge;
                }
                break;

            case EdgeDirection.Right:
                if ((x + 1) < this.width) {
                    this.points[y, x].rightEdge = edge;
                }
                break;

            case EdgeDirection.Down:
                if (y > 0) {
                    this.points[y - 1, x].upEdge = edge;
                }
                break;

            case EdgeDirection.Left:
                if (x > 0) {
                    this.points[y, x - 1].rightEdge = edge;
                }
                break;
        }
    }

    private bool HasEdgeAroundPointInternal(int x, int y, EdgeDirection direction) {
        return direction switch {
            EdgeDirection.Up => this.points[y, x].upEdge,
            EdgeDirection.Right => this.points[y, x].rightEdge,
            EdgeDirection.Down => y > 0 && this.points[y - 1, x].upEdge,
            EdgeDirection.Left => x > 0 && this.points[y, x - 1].rightEdge,
            _ => false,
        };
    }

    private bool IsOut(int x, int y) {
        return x < 0 || x >= this.width || y < 0 || y >= this.height;
    }

    public enum EdgeDirection {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    [Serializable]
    private struct Point {
        public bool upEdge;

        public bool rightEdge;
    }
}
