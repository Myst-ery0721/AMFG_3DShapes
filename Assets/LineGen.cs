using UnityEngine;

public class LineGen : MonoBehaviour
{
    public enum ShapeType
    {
        Square,
        RectangularColumn,
        Pyramid,
        Cylinder,
        Sphere,
        Capsule
    }

    [Header("Shape Settings")]
    public ShapeType currentShape = ShapeType.Square;
    public Material material;
    public float size = 5f;
    public Vector2 position;
    [Range(6, 64)] public int segments = 32;

    private void OnRenderObject()
    {
        if (material == null)
        {
            return;
        }

        material.SetPass(0);
        GL.PushMatrix();
        GL.Begin(GL.LINES);

        switch (currentShape)
        {
            case ShapeType.Square:
                DrawSquare();
                break;
            case ShapeType.RectangularColumn:
                DrawRectangularColumn();
                break;
            case ShapeType.Pyramid:
                DrawPyramid();
                break;
            case ShapeType.Cylinder:
                DrawCylinderShape(segments);
                break;
            case ShapeType.Sphere:
                DrawCircleShape(segments);
                break;
            case ShapeType.Capsule:
                DrawCapsuleShape(segments);
                break;
        }

        GL.End();
        GL.PopMatrix();
    }

    // SQUARE
    private void DrawSquare()
    {
        Vector2[] v =
        {
            position + new Vector2(-size * 0.5f, -size * 0.5f),
            position + new Vector2( size * 0.5f, -size * 0.5f),
            position + new Vector2( size * 0.5f,  size * 0.5f),
            position + new Vector2(-size * 0.5f,  size * 0.5f)
        };

        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(v[i]);
            GL.Vertex(v[(i + 1) % 4]);
        }
    }

    // RECTANGLE
    private void DrawRectangularColumn()
    {
        Vector2[] v =
        {
            position + new Vector2(-size * 0.4f, -size * 0.75f),
            position + new Vector2( size * 0.4f, -size * 0.75f),
            position + new Vector2( size * 0.4f,  size * 0.75f),
            position + new Vector2(-size * 0.4f,  size * 0.75f)
        };

        for (int i = 0; i < 4; i++)
        {
            GL.Vertex(v[i]);
            GL.Vertex(v[(i + 1) % 4]);
        }
    }

    // PYRAMID
    private void DrawPyramid()
    {
        Vector2 top = position + new Vector2(0, size * 0.6f);
        Vector2 left = position + new Vector2(-size * 0.5f, -size * 0.5f);
        Vector2 right = position + new Vector2(size * 0.5f, -size * 0.5f);

        GL.Vertex(left); GL.Vertex(top);
        GL.Vertex(top); GL.Vertex(right);
        GL.Vertex(right); GL.Vertex(left);
    }

    // CYLINDER
    private void DrawCylinderShape(int segments)
    {
        float radius = size * 0.4f;
        float halfHeight = size * 0.5f;

        // top ellipse
        Vector2[] top = new Vector2[segments];
        Vector2[] bottom = new Vector2[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.PI * 2f * i / segments;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius * 0.4f; 

            top[i] = position + new Vector2(x, halfHeight + y);
            bottom[i] = position + new Vector2(x, -halfHeight + y);
        }

        // draw ellipses
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            GL.Vertex(top[i]);
            GL.Vertex(top[next]);
            GL.Vertex(bottom[i]);
            GL.Vertex(bottom[next]);
        }

        // connect sides
        GL.Vertex(top[0]); GL.Vertex(bottom[0]);
        GL.Vertex(top[segments / 2]); GL.Vertex(bottom[segments / 2]);
    }

    // SPHERE
    private void DrawCircleShape(int segments)
    {
        float radius = size * 0.5f;
        for (int i = 0; i < segments; i++)
        {
            float angle0 = Mathf.PI * 2f * i / segments;
            float angle1 = Mathf.PI * 2f * (i + 1) / segments;
            Vector2 p0 = position + new Vector2(Mathf.Cos(angle0) * radius, Mathf.Sin(angle0) * radius);
            Vector2 p1 = position + new Vector2(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius);
            GL.Vertex(p0);
            GL.Vertex(p1);
        }
    }

    // CAPSULE
    private void DrawCapsuleShape(int segments)
    {
        float radius = size * 0.4f;
        float halfHeight = size * 0.8f - radius; 


        // rectangle sides
        GL.Vertex(position + new Vector2(-radius, -halfHeight));
        GL.Vertex(position + new Vector2(-radius, halfHeight));
        GL.Vertex(position + new Vector2(radius, -halfHeight));
        GL.Vertex(position + new Vector2(radius, halfHeight));

        // top semicircle
        for (int i = 0; i < segments / 2; i++)
        {
            float angle0 = Mathf.PI * i / (segments / 2);
            float angle1 = Mathf.PI * (i + 1) / (segments / 2);
            Vector2 p0 = position + new Vector2(Mathf.Cos(angle0) * radius, Mathf.Sin(angle0) * radius + halfHeight);
            Vector2 p1 = position + new Vector2(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius + halfHeight);
            GL.Vertex(p0);
            GL.Vertex(p1);
        }

        // bottom semicircle
        for (int i = 0; i < segments / 2; i++)
        {
            float angle0 = Mathf.PI * i / (segments / 2);
            float angle1 = Mathf.PI * (i + 1) / (segments / 2);
            Vector2 p0 = position + new Vector2(Mathf.Cos(angle0) * radius, -Mathf.Sin(angle0) * radius - halfHeight);
            Vector2 p1 = position + new Vector2(Mathf.Cos(angle1) * radius, -Mathf.Sin(angle1) * radius - halfHeight);
            GL.Vertex(p0);
            GL.Vertex(p1);
        }
    }
}
